namespace Domain.Abstractions;
public abstract class Entity
{
    public Guid Id { get; init; }
    
    public bool IsDeleted { get; set; }
    
    public DateTimeOffset CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    
    protected Entity(Guid id, bool isDeleted, DateTimeOffset createdDate, string? createdBy)
    {
        Id = id;
        IsDeleted = isDeleted;
        CreatedDate = createdDate;
        CreatedBy = createdBy;
    }

    protected Entity()
    {
    }
}