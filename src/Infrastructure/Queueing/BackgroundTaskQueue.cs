using Domain.Models;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
namespace Infrastructure.Queueing;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<FileMetadataEvent> _queue = Channel.CreateUnbounded<FileMetadataEvent>();

    public void QueueMetadata(FileMetadataEvent metadata)
    {
        _queue.Writer.TryWrite(metadata);
    }

    public async Task<FileMetadataEvent> DequeueAsync(CancellationToken token)
    {
        return await _queue.Reader.ReadAsync(token);
    }
}
