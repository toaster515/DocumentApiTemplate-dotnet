using System;
using Domain.Abstractions;

namespace Domain.Models;

public class FileRecord : Entity
{
    public string FileName { get; set; }
    public StorageProvider Provider { get; set; }
    public string ObjectKey { get; set; }
    public string Url { get; set; }
    public DateTime UploadedAt { get; set; }
}
