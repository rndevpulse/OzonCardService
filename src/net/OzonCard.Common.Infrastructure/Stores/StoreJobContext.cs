using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Core;
using OzonCard.Common.Worker.Stores;

namespace OzonCard.Common.Infrastructure.Stores;

public class StoreJobContext(DbContext context) : IStoreContext, IAsyncDisposable
{
    
    public async Task<TEntity?> GetItemAsync<TEntity>(Guid id, CancellationToken ct = default) where TEntity : class, IWithId<Guid> =>
        await context.Set<TEntity>().FirstOrDefaultAsync(x=>x.Id == id, ct);
    
    public async Task<TEntity?> GetItemAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) where TEntity : class, IWithId<Guid> =>
        await context.Set<TEntity>().FirstOrDefaultAsync(expression, ct);

   
    public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        CancellationToken ct = default) where TEntity : class, IWithId<Guid> =>
        await context.Set<TEntity>().Where(expression).ToListAsync(ct);

    public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(CancellationToken ct = default)
        where TEntity : class, IWithId<Guid> =>
        await context.Set<TEntity>().ToListAsync(ct);

    public void Append<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid> =>
        context.Set<TEntity>().AddRange(entities);

    public void Remove<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid> =>
        context.Set<TEntity>().RemoveRange(entities);
    

    public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IWithId<Guid> =>
        context.Set<TEntity>();


    public ValueTask DisposeAsync() 
    {
        return ValueTask.CompletedTask;
    }
}