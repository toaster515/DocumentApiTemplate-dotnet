using System;
using Domain.Models;
namespace Application.Models;

public class FileMetadataEvent
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ObjectKey { get; set; } = string.Empty;
    public StorageProvider Provider { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
