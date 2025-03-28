using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using System;
using System.Collections.Generic;
using Domain.Abstractions;

namespace Application.Interfaces;

public interface IFileRecordRepository : IRepository<FileRecord>
{
    Task<IReadOnlyList<FileRecord?>> GetAllAsync(CancellationToken cancellationToken = default);
}
