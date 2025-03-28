using System;
using Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;
using Application.Commands;
using Application.Models;

namespace Infrastructure.Queueing;

public class MetadataWorker : BackgroundService
{
    private readonly IBackgroundTaskQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MetadataWorker> _logger;

    public MetadataWorker(IBackgroundTaskQueue queue, IServiceScopeFactory scopeFactory, ILogger<MetadataWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var metadata = await _queue.DequeueAsync(stoppingToken);

            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var cmd = new CreateFileRecordCommand
            {
                FileName = metadata.FileName,
                ObjectKey = metadata.ObjectKey,
                Provider = metadata.Provider,
                Url = metadata.Url,
            };

            try
            {
                await mediator.Send(cmd, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process metadata record.");
            }
        }
    }
}
