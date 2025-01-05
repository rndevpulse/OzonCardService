using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OzonCard.Common.Infrastructure.Database.Contexts;
using OzonCard.Common.Infrastructure.Database.Extensions;

namespace OzonCard.Common.Infrastructure.Database.ContextFactories;

public class TaskContextFactory : IDesignTimeDbContextFactory<TaskContext>
{
    public TaskContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TaskContext>();
        optionsBuilder.ConfigureContext("database connection", "postgre");
        return new TaskContext(optionsBuilder.Options);
    }
}