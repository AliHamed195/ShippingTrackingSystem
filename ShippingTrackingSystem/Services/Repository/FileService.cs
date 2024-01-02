using ShippingTrackingSystem.Services.Interfaces;

namespace ShippingTrackingSystem.Services.Repository
{
    public class FileService : IFileService
    {
        public async Task<string?> AddFileAsync(IFormFile UploadedFile)
        {
            try
            {
                if (UploadedFile == null || UploadedFile.Length == 0)
                {
                    return null;
                }

                var fileName = Path.GetFileNameWithoutExtension(UploadedFile.FileName);
                var extension = Path.GetExtension(UploadedFile.FileName);
                var guid = Guid.NewGuid().ToString();
                var fileModelName = $"{guid}_{DateTime.Now:yyyyMMddHHmmss}_{fileName}_{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Images", fileModelName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }

                return $"/Images/{fileModelName}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteFileAsync(string FilePath)
        {
            try
            {
                // Check if the filePath starts with '/' and remove it
                if (FilePath.StartsWith("/"))
                {
                    FilePath = FilePath.TrimStart('/');
                }

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FilePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
