using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;

namespace Application.Queries;


public class GetFileQuery : IRequest<(Stream FileStream, string ContentType, string FileName)>
{
    public Guid Id { get; set; }
}


public class GetFileQueryHandler(
    IFileRecordRepository fileRecordRepository,
    IFileStorageFactory fileStorageFactory
) : IRequestHandler<GetFileQuery, (Stream, string, string)>
{
    public async Task<(Stream, string, string)> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        var record = await fileRecordRepository.GetByIdAsync(request.Id, cancellationToken);

        if (record == null)
            throw new KeyNotFoundException($"File with ID {request.Id} not found.");

        var storageService = fileStorageFactory.Get(record.Provider);
        var stream = await storageService.DownloadAsync(record.ObjectKey);

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(record.FileName, out var contentType))
            contentType = "application/octet-stream";

        return (stream, contentType, record.FileName);
    }
}
