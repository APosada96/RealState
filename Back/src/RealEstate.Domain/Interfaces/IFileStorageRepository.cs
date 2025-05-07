using Microsoft.AspNetCore.Http;

namespace RealEstate.Domain.Interfaces
{
    /// <summary>
    /// Abstraction for file storage operations such as saving and deleting images.
    /// </summary>
    public interface IFileStorageRepository
    {
        Task<string> SaveImageAsync(IFormFile file, string folder);
        Task DeleteImageAsync(string rute, string folder);
    }
}
