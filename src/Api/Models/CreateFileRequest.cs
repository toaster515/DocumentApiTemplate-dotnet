using Domain.Models;
namespace Api.Models;

public class CreateFileRequest
{
    public string FileName { get; set; } = string.Empty;
    public StorageProvider Provider { get; set; } = StorageProvider.Aws;
    public string UploadedBy { get; set; } = string.Empty; 
}
