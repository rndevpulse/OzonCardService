
using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;

namespace OzonCard.Context.Repositories
{
	public class ServiceRepository : BaseRepository, IServiceRepository
	{
		public ServiceRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory) { }

		public async Task<bool> CreateBackup(string path)
		{
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				var name_db = context.Database.GetDbConnection().Database;
				var name_bak = Path.Combine(path,
					string.Format("{0}_{1}.bak", name_db, DateTime.UtcNow.ToString("dd-MM-yyyy_HH_mm_ss")));
				await context.Database.ExecuteSqlRawAsync($"BACKUP DATABASE [{name_db}] TO DISK = '{name_bak}'");



				return true;
			}
		}

		public async Task<bool> RemoveOldFile(int countDays)
        {
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				var date = DateTime.UtcNow.AddDays(-countDays);
				var files = await context.FileReports
					.Where(x => x.Created > date)
					.ToListAsync();
				context.RemoveRange(files);
				await context.SaveChangesAsync();
				return true;
			}
		}

        public async Task<bool> RemoveOldTokensRefresh(int countDays)
        {
			using (var context = ContextFactory.CreateDbContext(ConnectionString))
			{
				var date = DateTime.UtcNow.AddDays(-countDays);
				var tokens = await context.Users
					.Include(x=>x.RefreshTokens)
					.SelectMany(x=>x.RefreshTokens)
					.Where(x => x.Expires > date)
					.ToListAsync();
				context.RemoveRange(tokens);
				await context.SaveChangesAsync();
				return true;
			}
		}
    }
}
