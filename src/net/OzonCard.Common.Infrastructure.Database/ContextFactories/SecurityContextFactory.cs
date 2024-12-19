using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OzonCard.Common.Infrastructure.Database.Contexts;
using OzonCard.Common.Infrastructure.Database.Extensions;

namespace OzonCard.Common.Infrastructure.Database.ContextFactories;

public class SecurityContextFactory : IDesignTimeDbContextFactory<SecurityContext>
{
    public SecurityContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SecurityContext>();
        optionsBuilder.ConfigureContext("database connection", "postgres");
        return new SecurityContext(optionsBuilder.Options);
    }
}