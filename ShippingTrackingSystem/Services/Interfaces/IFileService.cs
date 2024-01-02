namespace ShippingTrackingSystem.Services.Interfaces
{
    public interface IFileService
    {
        Task<string?> AddFileAsync(IFormFile UploadedFile);
        bool DeleteFileAsync(string FilePath);
    }
}
