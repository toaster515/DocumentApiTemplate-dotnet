using Domain.Models;
using System;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Application.Interfaces;
using Domain.Abstractions;

public class CreateFileRecordCommand : IRequest<Guid>
{
    public string FileName { get; set; } = string.Empty;
    public string ObjectKey { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public StorageProvider Provider { get; set; }
}

public class CreateFileRecordCommandHandler(
    IFileRecordRepository fileRecordRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateFileRecordCommand, Guid>
{
    public async Task<Guid> Handle(CreateFileRecordCommand request, CancellationToken cancellationToken)
    {
        var record = new FileRecord
        {
            Id = Guid.NewGuid(),
            FileName = request.FileName,
            ObjectKey = request.ObjectKey,
            Url = request.Url,
            Provider = request.Provider,
            UploadedAt = DateTime.UtcNow
        };

        fileRecordRepository.Add(record);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return record.Id;
    }
}