using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Worker.Domain.Jobs;

namespace OzonCard.Common.Infrastructure.Database;

public class JobContext(
    DbContextOptions<JobContext> options,
    ILogger<JobContext> logger)
    : DbContext(options), IEventTransactionManager
{
    private const string Schema = "task";

    
    private IDbContextTransaction? _transaction;
    private readonly ICollection<object> _disposables = new List<object>();

    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        builder.Entity<Job>(e =>
        {
            e.ToTable("jobs", Schema);
            
            e.HasKey(x => x.Id);
        
            e.Property(x => x.CreatedAt);
            e.Property(x => x.UpdatedAt);
            // e.Property(x => x.IsRemoved);
            //
            // e.HasQueryFilter(x => !x.IsRemoved);
            //
            e.Property(x => x.Number);
            e.Property(x => x.User);
            e.Property(x => x.Status);
            e.Property(x => x.Closed);
            e.Property(x => x.Progress);
            e.Property(x => x.Result);
            e.Property(x => x.Arguments);
        });
    }
    
    public bool StartTransaction()
    {
        if (_transaction != null)
            return false;

        _transaction = Database.BeginTransaction();
        return true;
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await SaveChangesAsync(ct);
            await _transaction.CommitAsync(ct);

            _transaction = null;
            _disposables.Clear();

            foreach (var disposable in ChangeTracker.Entries().Select(x => x.Entity).OfType<IDisposable>())
                _disposables.Add(disposable);

            foreach (var disposable in ChangeTracker.Entries().Select(x => x.Entity).OfType<IAsyncDisposable>())
                _disposables.Add(disposable);
        }
    }
    
    public override async ValueTask DisposeAsync()
    {
        foreach (var disposable in _disposables.OfType<IDisposable>())
            disposable.Dispose();

        foreach (var disposable in _disposables.OfType<IAsyncDisposable>())
            await disposable.DisposeAsync();

        await base.DisposeAsync();
    }
}