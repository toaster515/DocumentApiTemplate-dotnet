using System.Threading;
using System.Threading.Tasks;
using Application.Models;
namespace Application.Interfaces;
public interface IBackgroundTaskQueue
{
    void QueueMetadata(FileMetadataEvent metadata);
    Task<FileMetadataEvent> DequeueAsync(CancellationToken token);
}
