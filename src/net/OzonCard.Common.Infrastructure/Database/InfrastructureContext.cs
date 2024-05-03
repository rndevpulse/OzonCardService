using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace OzonCard.Common.Infrastructure.Database;

public class InfrastructureContext(
    DbContextOptions<InfrastructureContext> options,
    ILogger<InfrastructureContext> logger)
    : DbContext(options), ITransactionManager
{
    private readonly ICollection<object> _disposables = new List<object>();
    private IDbContextTransaction? _transaction;
    
    public event EventHandler<TransactionFailedEventArgs> TransactionFailed = delegate {  };
    public void HasError(Exception error) => TransactionFailed("InfrastructureContext", new TransactionFailedEventArgs(error));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
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