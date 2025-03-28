
namespace Domain.Abstractions;

public interface IRepository<T>
{
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public void Add(T entity);
    public void AddRange(IEnumerable<T> entities);
    public void Update(T entity);
    public void UpdateRange(IEnumerable<T> entities);
    IQueryable<T> Items { get; }
}
