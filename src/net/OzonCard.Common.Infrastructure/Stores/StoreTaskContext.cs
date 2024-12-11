using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Infrastructure.Stores;

public class StoreTaskContext : IStoreContext, IDisposable
{
    private readonly DbContext _context;
    private readonly IDbContextTransaction _transaction;

    public StoreTaskContext(DbContext context)
    {
        _context = context;
        _transaction = _context.Database.BeginTransaction();
    }

    public async Task<TEntity?> GetItemAsync<TEntity>(Guid id, CancellationToken ct = default) where TEntity : class, IWithId<Guid> =>
        await _context.Set<TEntity>().FirstOrDefaultAsync(x=>x.Id == id, ct);
    
    public async Task<TEntity?> GetItemAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) where TEntity : class, IWithId<Guid> =>
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
        _context.SaveChanges();
        _transaction.Commit();
    }
}