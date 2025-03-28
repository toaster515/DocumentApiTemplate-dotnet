using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;

namespace Application.Queries;

public sealed record GetRecordQuery(Guid Id) : IRequest<FileRecordDto>;

public class GetRecordQueryHandler(
    IFileRecordRepository fileRecordRepository
) : IRequestHandler<GetRecordQuery, FileRecordDto>
{
    public async Task<FileRecordDto> Handle(GetRecordQuery request, CancellationToken cancellationToken)
    {
        var record = await fileRecordRepository.GetByIdAsync(request.Id, cancellationToken);

        if (record == null)
            throw new KeyNotFoundException($"File with ID {request.Id} not found.");

        var recordDto = new FileRecordDto
        {
            FileName = record.FileName,
            Provider = record.Provider,
            Url = record.Url
        };

        return recordDto;
    }
}