using System.Linq.Expressions;

namespace OzonCard.Common.Core;




public interface IEntityStoreWithId<in TKey> where TKey : struct
{
    IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IWithId<TKey>;
    Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) where TEntity : class, IWithId<TKey>;
    Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(CancellationToken ct = default) where TEntity : class, IWithId<TKey>;
    Task<TEntity?> GetItemAsync<TEntity>(TKey id, CancellationToken ct = default) where TEntity : class, IWithId<TKey>;
    
    Task<TEntity?> GetItemAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) where TEntity : class, IWithId<TKey>;
    
    void Append<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<TKey>;
    void Remove<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<TKey>;

}