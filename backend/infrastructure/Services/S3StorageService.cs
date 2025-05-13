using domain.Interfaces;

namespace infrastructure.Services;

public class S3StorageService: ICloudStorageService
{
    private readonly S3Options _s3Options;
    private readonly ILogger<S3StorageService> _logger;
    private readonly S3Client _s3Client
    
    public S3StorageService(S3Options s3Options, ILogger<S3StorageService> logger)
    {
        _s3Options = s3Options;
        _logger = logger;
        _s3Client = new S3Client(s3Options);
    }
    
    public Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string folder = null)
    {
        // Validation
        // url = await S3Client.Upload(fileStream, fileName, contentType, folder);
        // return url
        throw new NotImplementedException();
    }

    public Task DeleteFileAsync(string fileUrlOrKey)
    {
        // Validation
        // await S3Client.Delete(fileUrlOrKey);
        throw new NotImplementedException();
    }
}