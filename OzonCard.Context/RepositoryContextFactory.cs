using Microsoft.EntityFrameworkCore;
using OzonCard.Context.Interfaces;

namespace OzonCard.Context
{
	public class RepositoryContextFactory : IRepositoryContextFactory
	{
		public RepositoryContext CreateDbContext(string connectionString)
		{
			var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
			optionsBuilder.UseSqlServer(connectionString);
			//optionsBuilder.LogTo(System.Console.WriteLine);
			return new RepositoryContext(optionsBuilder.Options);
		}
	}
}
