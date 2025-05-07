using Microsoft.AspNetCore.Http;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of <see cref="IFileStorageRepository"/> that handles saving and deleting image files
    /// in the local file system under the 'wwwroot' folder. Also generates public URLs for uploaded files.
    /// </summary>
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorageRepository"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context for building URLs.</param>
        public FileStorageRepository(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Saves an uploaded image file to the local file system under a specified folder.
        /// Generates a unique name for the file and returns the public URL path.
        /// </summary>
        /// <param name="file">The image file to save.</param>
        /// <param name="folder">The subfolder in 'wwwroot' where the image will be stored.</param>
        /// <returns>The publicly accessible URL to the saved image.</returns>
        public async Task<string> SaveImageAsync(IFormFile file, string folder)
        {
            var extension = Path.GetExtension(file.FileName);
            var nameFile = $"{Guid.NewGuid()}{extension}";


            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var rute = Path.Combine(rootPath, nameFile);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(rute, content);
            }

            var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            return Path.Combine(url, folder, nameFile).Replace("\\", "/");

        }

        /// <summary>
        /// Deletes an image file from the local file system if it exists.
        /// </summary>
        /// <param name="rute">The relative or full path to the image file to delete.</param>
        /// <param name="folder">The folder where the file is stored.</param>
        public Task DeleteImageAsync(string rute, string folder)
        {
            if (string.IsNullOrEmpty(rute))
            {
                return Task.CompletedTask;
            }

            var nameFile = Path.GetFileName(rute);
            var directoryFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, nameFile);

            if (File.Exists(directoryFile))
            {
                File.Delete(directoryFile);
            }

            return Task.CompletedTask;
        }
    }
}
