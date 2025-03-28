using System;
using Domain.Models;
namespace Application.Models;

public class FileRecordDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public StorageProvider Provider { get; set; }
    public string Url { get; set; }
    public DateTime UploadedAt { get; set; }
}
