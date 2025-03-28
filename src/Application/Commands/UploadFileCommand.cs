using Application.Interfaces;
using Application.Models;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using System.IO;

namespace Application.Commands;
public class UploadFileCommand : IRequest<CreateFileDto>
{
    public Stream FileStream { get; set; } = default!;
    public string ContentType { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;
    public StorageProvider Provider { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
}

public class UploadFileCommandHandler(
    IFileStorageFactory storageFactory,
    IBackgroundTaskQueue taskQueue
) : IRequestHandler<UploadFileCommand, CreateFileDto>
{
    public async Task<CreateFileDto> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var storageService = storageFactory.Get(request.Provider);

        string objectKey = Guid.NewGuid().ToString();
        string url = await storageService.UploadAsync(request.FileStream, objectKey, request.ContentType);

        // Save metadata to DB
        taskQueue.QueueMetadata(new FileMetadataEvent
        {
            Id = Guid.NewGuid(),
            UploadedAt = DateTime.UtcNow,
            FileName = request.FileName,
            ObjectKey = objectKey,
            Provider = request.Provider,
            Url = url,
        });

        return new CreateFileDto
        {
            FileName = request.FileName,
            ObjectKey = objectKey,
            Url = url
        };
    }
}