using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OzonCard.Common.Core;

namespace OzonCard.Common.Infrastructure.Stores;
//
// public class SimpleStore<TContext> : ISimpleStore where TContext : DbContext
// {
//     private readonly string _connection;
//
//     public SimpleStore(string connection)
//     {
//         _connection = connection;
//     }
//
//     public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class, IWithId<Guid>
//     {
//         using (var context = new TContext(b=>b.))
//         {
//             throw new NotImplementedException();
//         }
//     }
//
//     public Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) where TEntity : class, IWithId<Guid>
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<IEnumerable<TEntity>> GetItemsAsync<TEntity>(CancellationToken ct = default) where TEntity : class, IWithId<Guid>
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<TEntity?> GetItemAsync<TEntity>(Guid id, CancellationToken ct = default) where TEntity : class, IWithId<Guid>
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<TEntity?> GetItemAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default) where TEntity : class, IWithId<Guid>
//     {
//         throw new NotImplementedException();
//     }
//
//     public void Append<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid>
//     {
//         throw new NotImplementedException();
//     }
//
//     public void Remove<TEntity>(params TEntity[] entities) where TEntity : class, IWithId<Guid>
//     {
//         throw new NotImplementedException();
//     }
//
//     public void Dispose()
//     {
//         throw new NotImplementedException();
//     }
// }