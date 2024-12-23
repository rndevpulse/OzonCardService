using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Core;
using OzonCard.Common.Infrastructure.Database.Contexts;

namespace OzonCard.Common.Infrastructure.Stores;

public class StoreTaskContext : IStoreContext
{
    private readonly  DbContextOptions<TaskContext> _options;
    private readonly ILogger<StoreTaskContext> _logger;

    public StoreTaskContext(DbContextOptions options, ILogger<StoreTaskContext> logger)
    {
        _options = options as DbContextOptions<TaskContext>
            ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    public async Task<TEntity?> GetItemAsync<TEntity>(Guid id, CancellationToken ct = default)
        where TEntity : class, IWithId<Guid>
    {
        await using var context = new TaskContext(_options);
        {
            return  await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, ct);
        }
    }


    public async Task<TEntity?> GetItemAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        CancellationToken ct = default) where TEntity : class, IWithId<Guid>
    {
        await using var context = new TaskContext(_options);
        {
            return await context.Set<TEntity>().FirstOrDefaultAsync(expression, ct);
        }
    }



    public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(Expression<Func<TEntity, bool>> expression,
        CancellationToken ct = default) where TEntity : class, IWithId<Guid>
    {
        await using var context = new TaskContext(_options);
        {
            return await context.Set<TEntity>().Where(expression).ToListAsync(ct);
        }
    }


    public async Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(CancellationToken ct = default)
        where TEntity : class, IWithId<Guid>
    {
        await using var context = new TaskContext(_options);
        {
            return await context.Set<TEntity>().ToListAsync(ct);
        }
    }

    public void Append<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid>
    {
        using var context = new TaskContext(_options);
        {
            context.Set<TEntity>().AddRange(entities);
            context.SaveChanges();
        }
    }

    public void Remove<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid>
    {
        using var context = new TaskContext(_options);
        {
            context.Set<TEntity>().RemoveRange(entities);
            context.SaveChanges();
        }
    }

    public void Update<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid>
    {
        using var context = new TaskContext(_options);
        {
            context.Set<TEntity>().UpdateRange(entities);
            context.SaveChanges();
        }
    }


    public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IWithId<Guid> =>
        throw new NotSupportedException();


    public void Dispose()
    {
       
    }
}