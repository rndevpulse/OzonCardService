

namespace OzonCard.Common.Core;

public interface IRepository<TEntity, in TKey>
    where TEntity : IWithId<TKey>
    where TKey : struct
{
    TEntity? TryGetItem(Guid key);
    Task<TEntity?> TryGetItemAsync(Guid key, CancellationToken ct = default);
    IQueryable<TEntity> GetQuery();
    TEntity GetItem(TKey key);
    Task<TEntity> GetItemAsync(TKey key, CancellationToken ct = default);
    Task<IEnumerable<TEntity>> GetItemsAsync(CancellationToken ct = default);
    void Add(params TEntity[] entities);
    Task AddAsync(params TEntity[] entities);
    void Update(params TEntity[] entities);
    void Remove(params TEntity[] entities);
}

public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : IWithId<Guid>
{
}