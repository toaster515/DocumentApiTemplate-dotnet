using System.Reflection.PortableExecutable;
using Domain.Models;
using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces;
public interface IFileStorageService
{
    StorageProvider Provider { get; }
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task<Stream> DownloadAsync(string objectKey);
}
