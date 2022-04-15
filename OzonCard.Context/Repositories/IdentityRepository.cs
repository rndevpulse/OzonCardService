

using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;
using OzonCard.Data.Models;

namespace OzonCard.Context.Repositories
{
    public class IdentityRepository : BaseRepository, IIdentityRepository
	{
		public IdentityRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory) { }

		public async Task<User?> GetUser(string userName, Guid password)
		{
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				return await context.Users.FirstOrDefaultAsync(x => x.Mail == userName && x.Password == password);
			}
		}



		public async  Task<bool> AddRefreshToken(User user, RefreshToken? refreshToken = null)
        {
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				if (refreshToken != null)
					user.RefreshTokens.Add(refreshToken);
				context.Update(user);
				await context.SaveChangesAsync();
				return true;
			}
		}

        public async Task<User?> GetUser(string refreshToken)
        {
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				return await context.Users
					.Include(x => x.RefreshTokens)
					.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
			}
		}
    }
}
