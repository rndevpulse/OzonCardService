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
		public DbSet<FileReport> FileReports { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Event> Events { get; set; }


		public static void InitializationValue(MigrationBuilder migrationBuilder)
		{
            var rules = string.Join(',', EnumUtil.GetAllValues<EnumRules>().Select(x=>(int)x));
			migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Mail", "Rules", "CreatedDate", "Password" },
                values: new object[] { Guid.NewGuid(), "admin", rules, DateTime.UtcNow, "b02a44c9-774d-cdbb-fbac-f2d424c0625c" });

        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");
			modelBuilder.Entity<Organization>().Property(u=>u.Login).IsRequired();
			modelBuilder.Entity<Organization>().Property(u=>u.Password).IsRequired();

			modelBuilder.Entity<CategoryCustomer>()
		   .HasKey(t => new { t.CustomerId, t.CategoryId });

			modelBuilder.Entity<CategoryCustomer>()
				.HasOne(sc => sc.Customer)
				.WithMany(s => s.Categories)
				.HasForeignKey(sc => sc.CustomerId);

			modelBuilder.Entity<CategoryCustomer>()
				.HasOne(sc => sc.Category)
				.WithMany(c => c.Customers)
				.HasForeignKey(sc => sc.CategoryId);
		}

    }
}