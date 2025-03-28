using Microsoft.EntityFrameworkCore;
using Domain.Models;
using System.Text.Json;
using Domain.Abstractions;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace Infrastructure.Storage;

public sealed class AppDbContext(
    DbContextOptions options)
    : DbContext(options), IUnitOfWork
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DBConcurrencyException("Concurrency exception occurred.", ex);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FileRecord>(entity =>
        {
            entity.ToTable("file_records");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired();
            entity.Property(e => e.Url).IsRequired();
            entity.Property(e => e.ObjectKey).IsRequired();
            entity.Property(e => e.Provider).HasConversion<string>();
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("now()");
        });
    }
}
