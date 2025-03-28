using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using System.Threading.Tasks;
using System.IO;
using Domain.Models;

namespace Infrastructure.Storage;

public class AzureBlobStorageService : IFileStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _config;
    private readonly ILogger<AzureBlobStorageService> _logger;
    public StorageProvider Provider => StorageProvider.Azure;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient, IConfiguration config, ILogger<AzureBlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _config = config;
        _logger = logger;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var containerName = _config["Storage:Azure:ContainerName"];
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

        _logger.LogInformation($"Uploaded file {fileName} to Azure Blob.");

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string objectKey)
    {
        var containerName = _config["Storage:Azure:ContainerName"];
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(objectKey);

        var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        memoryStream.Position = 0;

        _logger.LogInformation($"Downloaded {objectKey} from Azure Blob.");
        return memoryStream;
    }

}
