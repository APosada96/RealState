using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces
{
    /// <summary>
    /// Defines data access operations for Property entities.
    /// </summary>
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetAllAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice);
        Task<Property?> GetByIdAsync(string id);
        Task AddAsync(Property property);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string name, string address);
    }
}
