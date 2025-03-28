using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;
using System.Threading.Tasks;
using System.IO;
using Domain.Models;

namespace Infrastructure.Storage;

public class AwsStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _config;
    private readonly ILogger<AwsStorageService> _logger;
    public StorageProvider Provider => StorageProvider.Aws;

    public AwsStorageService(IAmazonS3 s3Client, IConfiguration config, ILogger<AwsStorageService> logger)
    {
        _s3Client = s3Client;
        _config = config;
        _logger = logger;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var bucketName = _config["Storage:Aws:BucketName"];

        var fileTransferUtility = new TransferUtility(_s3Client);

        var request = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = fileName,
            BucketName = bucketName,
            ContentType = contentType
        };

        await fileTransferUtility.UploadAsync(request);
        _logger.LogInformation($"Uploaded file {fileName} to S3.");

        return $"s3://{bucketName}/{fileName}";
    }
    
    public async Task<Stream> DownloadAsync(string objectKey)
    {
        var bucketName = _config["Storage:Aws:BucketName"];

        var response = await _s3Client.GetObjectAsync(bucketName, objectKey);
        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0; // reset stream position for downstream use

        _logger.LogInformation($"Downloaded {objectKey} from S3.");
        return memoryStream;
    }

}
