using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Abstractions;
using System.Diagnostics.CodeAnalysis;
namespace Infrastructure.Storage;

[ExcludeFromCodeCoverage]
internal abstract class Repository<T>(AppDbContext dbContext) : IRepository<T>
    where T : Entity
{
    protected readonly AppDbContext DbContext = dbContext;

    public virtual async Task<T?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<T>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<T?> GetReadOnlyByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual void Add(T entity)
    {
        DbContext.Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        DbContext.AddRange(entities);
    }

    public virtual void Update(T entity)
    {
        DbContext.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        DbContext.UpdateRange(entities);
    }

    public IQueryable<T> Items => DbContext.Set<T>().AsNoTracking();
}