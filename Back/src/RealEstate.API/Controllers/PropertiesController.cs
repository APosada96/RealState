using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;

namespace RealEstate.API.Controllers
{
    /// <summary>
    /// Property management controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyService _service;
        private readonly IMapper _mapper;
        public PropertiesController(PropertyService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of properties, optionally filtered by name, address, or price range.
        /// </summary>
        /// <param name="name">Filter by name.</param>
        /// <param name="address">Filter by address.</param>
        /// <param name="minPrice">Minimum price.</param>
        /// <param name="maxPrice">Maximum price.</param>
        /// <returns>List of matching properties.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] string? name, [FromQuery] string? address, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var result = await _service.GetPropertiesAsync(name, address, minPrice, maxPrice);
            var dtoList = _mapper.Map<IEnumerable<PropertyResponseDto>>(result);
            return Ok(dtoList);
        }

        /// <summary>
        /// Retrieves details of a property by its ID.
        /// </summary>
        /// <param name="id">Property ID.</param>
        /// <returns>Property details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var property = await _service.GetPropertyByIdAsync(id);
            if (property == null) return NotFound();
            var dto = _mapper.Map<PropertyResponseDto>(property);
            return Ok(dto);
        }

        /// <summary>
        /// Creates a new property record.
        /// </summary>
        /// <param name="dto">Data for the new property.</param>
        /// <returns>The created property.</returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromForm] PropertyCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreatePropertyWithImageAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex) when (ex.Message.Contains("A property already exists"))
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", detail = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a property by its ID.
        /// </summary>
        /// <param name="id">The ID of the property to delete.</param>
        /// <returns>NoContent if successful, NotFound if the property does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeletePropertyAsync(id);
                return NoContent();
            }
            catch (Exception ex) when (ex.Message.Contains("does not exist"))
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
