using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OzonCard.Context.Interfaces;
using OzonCard.Logger;
using Serilog;
using System.IO;

namespace OzonCardService
{
    public class Program
    {
        public static void Main(string[] args)
		{
			new InitialSerilogger();


			var host = BuildWebHost(args);
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.Development.json", true, true)
				.AddJsonFile($"appsettings.production.json", true);
			var config = builder.Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var factory = services.GetRequiredService<IRepositoryContextFactory>();
				factory.CreateDbContext(config.GetConnectionString("DefaultConnection")).Database.Migrate();
				
			}

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
		   WebHost.CreateDefaultBuilder(args)
			.UseSerilog()
			.UseStartup<Startup>()
			.Build();
	}
}
