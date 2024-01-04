namespace ShippingTrackingSystem.Services.Interfaces
{
    /// <summary>
    /// Interface for file management services.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Adds a file asynchronously to the system storage.
        /// </summary>
        /// <param name="UploadedFile">The file to be uploaded.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the path of the uploaded file if successful; 
        /// otherwise, null.
        /// </returns>
        Task<string?> AddFileAsync(IFormFile UploadedFile);

        /// <summary>
        /// Deletes a file asynchronously from the system storage.
        /// </summary>
        /// <param name="FilePath">The path of the file to be deleted.</param>
        /// <returns>True if the file was successfully deleted; otherwise, false.</returns>
        bool DeleteFileAsync(string FilePath);
    }
}
