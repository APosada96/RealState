using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Services
{
    /// <summary>
    /// Application service responsible for business logic related to properties.
    /// </summary>
    public class PropertyService
    {
        private readonly IPropertyRepository _repository;
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyService"/> class.
        /// </summary>
        /// <param name="repository">The repository for accessing property data.</param>
        /// <param name="fileStorageRepository">The file storage repository for handling image files.</param>
        /// <param name="mapper">The AutoMapper instance used for DTO/entity mapping.</param>
        public PropertyService(IPropertyRepository repository, IFileStorageRepository fileStorageRepository, IMapper mapper)
        {
            _repository = repository;
            _fileStorageRepository = fileStorageRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a filtered list of properties.
        /// </summary>
        /// <param name="name">Optional name filter.</param>
        /// <param name="address">Optional address filter.</param>
        /// <param name="minPrice">Optional minimum price filter.</param>
        /// <param name="maxPrice">Optional maximum price filter.</param>
        /// <returns>A collection of matching properties.</returns>
        public async Task<IEnumerable<Property>> GetPropertiesAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            return await _repository.GetAllAsync(name, address, minPrice, maxPrice);
        }

        /// <summary>
        /// Retrieves a property by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the property.</param>
        public Task<Property?> GetPropertyByIdAsync(string id)
        {
            return _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new property, saves its associated image, and returns the created DTO.
        /// </summary>
        /// <param name="dto">The property creation DTO containing data and the image file.</param>
        /// <returns>The created property's response DTO.</returns>
        public async Task<PropertyResponseDto> CreatePropertyWithImageAsync(PropertyCreateDto dto)
        {
            bool exists = await _repository.ExistsAsync(dto.Name, dto.Address);
            if (exists)
                throw new Exception("A property already exists");


            var imageUrl = await _fileStorageRepository.SaveImageAsync(dto.Image, "images");

            var property = _mapper.Map<Property>(dto);
            property.ImageUrl = imageUrl;

            await _repository.AddAsync(property);

            var response = _mapper.Map<PropertyResponseDto>(property);
            return response;
        }

        /// <summary>
        /// Deletes a property and its associated image from the system.
        /// </summary>
        /// <param name="id">The ID of the property to delete.</param>
        /// <returns>True if the property was successfully deleted.</returns>
        public async Task<bool> DeletePropertyAsync(string id)
        {
            var property = await _repository.GetByIdAsync(id);
            if (property == null)
                throw new Exception("The property does not exist.");

            await _fileStorageRepository.DeleteImageAsync(property.ImageUrl, "images");

            return await _repository.DeleteAsync(id);
        }

    }
}
