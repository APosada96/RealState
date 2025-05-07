using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RealEstate.API.Controllers;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.Text;

namespace RealEstate.Tests
{
    /// <summary>
    /// Unit tests for PropertiesController.
    /// </summary>
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _mockRepo = null!;
        private Mock<IFileStorageRepository> _mockFile = null!;
        private Mock<IMapper> _mockMapper = null!;
        private PropertyService _service = null!;
        private PropertiesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyRepository>();
            _mockFile = new Mock<IFileStorageRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new PropertyService(_mockRepo.Object, _mockFile.Object, _mockMapper.Object);
            _controller = new PropertiesController(_service, _mockMapper.Object);
        }

        /// <summary>
        /// Tests that the Get method returns 200 OK with a list of properties.
        /// </summary>
        [Test]
        public async Task Get_ReturnsOk_WithList()
        {
            var properties = new List<Property> { new Property { Id = "1", Name = "Test" } };
            var responseDtos = new List<PropertyResponseDto> { new PropertyResponseDto { Id = "1", Name = "Test" } };

            _mockRepo.Setup(r => r.GetAllAsync(null, null, null, null)).ReturnsAsync(properties);
            _mockMapper.Setup(m => m.Map<IEnumerable<PropertyResponseDto>>(properties)).Returns(responseDtos);

            var result = await _controller.Get(null, null, null, null);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<PropertyResponseDto>>(okResult!.Value);
        }

        /// <summary>
        /// Tests that GetById returns 200 OK when the property is found.
        /// </summary>
        [Test]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var property = new Property { Id = "1", Name = "Found" };
            var dto = new PropertyResponseDto { Id = "1", Name = "Found" };

            _mockRepo.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(property);
            _mockMapper.Setup(m => m.Map<PropertyResponseDto>(property)).Returns(dto);

            var result = await _controller.GetById("1");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<PropertyResponseDto>(okResult!.Value);
        }

        /// <summary>
        /// Tests that GetById returns 404 NotFound when the property is not found.
        /// </summary>
        [Test]
        public async Task GetById_ReturnsNotFound_WhenNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync("123")).ReturnsAsync((Property?)null);

            var result = await _controller.GetById("123");

            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task CreatePropertyWithImageAsync_MapsDtoAndReturnsResponse()
        {
            // Arrange: simulate an image file
            var content = "fake image content";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var formFile = new FormFile(stream, 0, stream.Length, "image", "test.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            var dto = new PropertyCreateDto
            {
                IdOwner = "owner-xyz",
                Name = "Test Home",
                Address = "456 Fake St",
                Price = 150000,
                Image = formFile
            };

            var mappedEntity = new Property
            {
                Id = Guid.NewGuid().ToString(),
                IdOwner = dto.IdOwner,
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                ImageUrl = string.Empty
            };

            _mockFile.Setup(f => f.SaveImageAsync(dto.Image, "images"))
                            .ReturnsAsync("/images/test.png");

            _mockMapper.Setup(m => m.Map<Property>(dto)).Returns(mappedEntity);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Property>())).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PropertyResponseDto>(It.IsAny<Property>()))
                      .Returns((Property p) => new PropertyResponseDto
                      {
                          Id = p.Id,
                          IdOwner = p.IdOwner,
                          Name = p.Name,
                          Address = p.Address,
                          Price = p.Price,
                          ImageUrl = "/images/test.png"
                      });

            // Act
            var result = await _service.CreatePropertyWithImageAsync(dto);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(dto.Name));
            Assert.That(result.Address, Is.EqualTo(dto.Address));
            Assert.IsTrue(result.ImageUrl.StartsWith("/images/"));
        }


        /// <summary>
        /// Tests that Post returns 400 BadRequest when the model state is invalid.
        /// </summary>
        [Test]
        public async Task Post_ReturnsBadRequest_WhenModelInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var dto = new PropertyCreateDto();

            var result = await _controller.Post(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        /// <summary>
        /// Unit test for successful deletion of an existing property.
        /// Ensures the controller returns HTTP 204 NoContent when the property exists,
        /// its image is deleted, and the deletion operation succeeds.
        /// </summary>
        [Test]
        public async Task Delete_WhenPropertyExists_ReturnsNoContent()
        {
            _mockRepo.Setup(r => r.GetByIdAsync("123"))
             .ReturnsAsync(new Property
             {
                 Id = "123",
                 Name = "Test",
                 Address = "X",
                 Price = 1000,
                 IdOwner = "owner",
                 ImageUrl = "/images/test.png"
             });

            _mockFile.Setup(f => f.DeleteImageAsync("/images/test.png", "images"))
                 .Returns(Task.CompletedTask);


            _mockRepo.Setup(s => s.DeleteAsync("123")).ReturnsAsync(true);
            var result = await _controller.Delete("123");
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        /// <summary>
        /// Unit test for deletion attempt of a non-existent property.
        /// Ensures the controller catches the exception and returns HTTP 404 NotFound
        /// with a descriptive message.
        /// </summary>
        [Test]
        public async Task Delete_WhenPropertyNotFound_ThrowsException_ReturnsNotFound()
        {
            _mockRepo.Setup(s => s.DeleteAsync("999"))
                        .ThrowsAsync(new Exception("The property does not exist."));

            var result = await _controller.Delete("999");

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.That(notFoundResult!.StatusCode, Is.EqualTo(404));
            Assert.That(notFoundResult.Value?.ToString(), Does.Contain("does not exist"));
        }
    }
}