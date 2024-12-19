using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OzonCard.Common.Infrastructure.Database.Contexts;
using OzonCard.Common.Infrastructure.Database.Extensions;

namespace OzonCard.Common.Infrastructure.Database.ContextFactories;

public class InfrastructureContextFactory : IDesignTimeDbContextFactory<InfrastructureContext>
{
    public InfrastructureContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InfrastructureContext>();
        optionsBuilder.ConfigureContext("database connection", "postgres");
        return new InfrastructureContext(optionsBuilder.Options);
    }
}