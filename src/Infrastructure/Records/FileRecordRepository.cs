using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Storage;
[ExcludeFromCodeCoverage]

internal sealed class FileRecordRepository(AppDbContext dbContext)
     : Repository<FileRecord>(dbContext), IFileRecordRepository
{

    public async Task<IReadOnlyList<FileRecord?>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<FileRecord>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
