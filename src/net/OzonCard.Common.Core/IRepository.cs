

namespace OzonCard.Common.Core;

public interface IRepository<TEntity, in TKey>
    where TEntity : IWithId<TKey>
    where TKey : struct
{
    public IQueryable<TEntity> GetQuery();
    public TEntity GetItem(TKey key);
    public Task<TEntity> GetItemAsync(TKey key, CancellationToken ct = default);
    public Task<IEnumerable<TEntity>> GetItemsAsync(CancellationToken ct = default);
    public void Add(params TEntity[] entities);
    public Task AddAsync(params TEntity[] entities);
    public void Update(params TEntity[] entities);
    public void Remove(params TEntity[] entities);
}

public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : IWithId<Guid>
{
}