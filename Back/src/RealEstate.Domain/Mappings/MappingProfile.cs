using AutoMapper;
using RealEstate.Domain.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Mappings
{
    /// <summary>
    /// AutoMapper profile that defines object-object mappings for the application.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Configures mappings between DTOs and domain models.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<PropertyCreateDto, Property>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Property, PropertyResponseDto>();
        }
    }
}
