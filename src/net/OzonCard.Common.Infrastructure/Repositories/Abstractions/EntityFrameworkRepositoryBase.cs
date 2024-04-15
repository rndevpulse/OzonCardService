using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Core;
using OzonCard.Common.Core.Exceptions;
using OzonCard.Common.Infrastructure.Database;

namespace OzonCard.Common.Infrastructure.Repositories.Abstractions;

public abstract class EntityFrameworkRepositoryBase<TEntity>(InfrastructureContext context) : IRepository<TEntity>
    where TEntity : class, IWithId<Guid>
{
    public IQueryable<TEntity> GetQuery() =>
        context.Set<TEntity>();

    private TEntity? TryGetItem(Guid key) => 
        context.Set<TEntity>().Find(key);
    private async Task<TEntity?> TryGetItemAsync(Guid key, CancellationToken ct = default) =>
        await context.Set<TEntity>().FindAsync(new object?[] { key }, ct);

    public TEntity GetItem(Guid key)
    {
        return TryGetItem(key) ?? throw EntityNotFoundException.For<TEntity>(key);
    }

    public async Task<TEntity> GetItemAsync(Guid key, CancellationToken ct = default)
    {
        return await TryGetItemAsync(key, ct) ?? throw EntityNotFoundException.For<TEntity>(key);
    }

    public async Task<IEnumerable<TEntity>> GetItemsAsync(CancellationToken ct = default)
    {
        return await context.Set<TEntity>().ToListAsync(ct);
    }

    public void Add(params TEntity[] entities)
    {
        context.AddRange(entities.ToList());
    }

    public async Task AddAsync(params TEntity[] entities)
    {
        await context.AddRangeAsync(entities.ToList());
    }

    public void Update(params TEntity[] entities)
    {
        context.UpdateRange(entities.ToList());
    }

    public void Remove(params TEntity[] entities)
    {
        context.RemoveRange(entities.ToList());
    }
}