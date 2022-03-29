using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using OzonCard.Common;
using OzonCard.Data.Enums;
using OzonCard.Data.Models;

namespace OzonCard.Context
{
    public class RepositoryContext : DbContext
	{

	//	private readonly static ILogger log = Log.ForContext(typeof(DatabaseContext));
		public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
		{

		}

		public DbSet<Card> Cards { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CorporateNutrition> CorporateNutritions { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Organization> Organizations { get; set; }
		public DbSet<CustomerWallet> CustomerWallets { get; set; }
		public DbSet<Wallet> Wallets { get; set; }
		public DbSet<User> Users { get; set; }
		public static void InitializationValue(MigrationBuilder migrationBuilder)
		{
            var rules = string.Join(',', EnumUtil.GetAllValues<EnumRules>().Select(x=>(int)x));
			migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Mail", "Rules", "CreatedDate", "Password" },
                values: new object[] { Guid.NewGuid(), "admin", "admin", rules, DateTime.UtcNow, "b02a44c9-774d-cdbb-fbac-f2d424c0625c" });

        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //log.Information("Creating context configuration");
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");
			modelBuilder.Entity<Organization>().Property(u=>u.Login).IsRequired();
			modelBuilder.Entity<Organization>().Property(u=>u.Password).IsRequired();



		}

    }
}