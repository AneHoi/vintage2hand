namespace domain.Interfaces;

public interface ICloudStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName,
        string contentType, string folder = null);

    Task DeleteFileAsync(string fileUrlOrKey);
}