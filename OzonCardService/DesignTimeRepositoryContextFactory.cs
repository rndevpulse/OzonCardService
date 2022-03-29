using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OzonCard.Context;
using System.IO;

namespace OzonCardService
{
	public class DesignTimeRepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
	{
		public RepositoryContext CreateDbContext(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.Development.json", true, true)
				.AddJsonFile($"appsettings.production.json", true)
				.Build();

			var connectionString = config.GetConnectionString("DefaultConnection");
			var repositoryFactory = new RepositoryContextFactory();

			return repositoryFactory.CreateDbContext(connectionString);
		}
	}
}
