using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Infrastructure.Stores;

public class StoreTaskContext : IStoreContext
{
    private readonly DbContext _context;
    private readonly ILogger<StoreTaskContext> _logger;
    private IDbContextTransaction _transaction;

    public StoreTaskContext(DbContext context, ILogger<StoreTaskContext> logger)
    {

        _context = context;
        _logger = logger;
        _transaction = _context.Database.BeginTransaction();
        // _logger.LogInformation($"StoreTaskContext start transaction");
    }

    public async Task<TEntity?> GetItemAsync<TEntity>(Guid id, CancellationToken ct = default)
        where TEntity : class, IWithId<Guid> =>
        await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<TEntity?> GetItemAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        CancellationToken ct = default) where TEntity : class, IWithId<Guid> =>
        await _context.Set<TEntity>().FirstOrDefaultAsync(expression, ct);


    public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        CancellationToken ct = default) where TEntity : class, IWithId<Guid> =>
        await _context.Set<TEntity>().Where(expression).ToListAsync(ct);

    public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(CancellationToken ct = default)
        where TEntity : class, IWithId<Guid> =>
        await _context.Set<TEntity>().ToListAsync(ct);

    public void Append<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid> =>
        _context.Set<TEntity>().AddRange(entities);

    public void Remove<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid> =>
        _context.Set<TEntity>().RemoveRange(entities);


    public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IWithId<Guid> =>
        _context.Set<TEntity>();


    public void Dispose()
    {
        if (_transaction != null)
        {
            _context.SaveChanges();
            _transaction.Commit();
            // _logger.LogInformation($"StoreTaskContext save transaction");
            _transaction = null;
        }
    }
}