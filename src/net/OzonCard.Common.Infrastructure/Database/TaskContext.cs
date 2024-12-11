using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OzonCard.Common.Worker.Domain.Jobs;

namespace OzonCard.Common.Infrastructure.Database;

public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
{
    private const string Schema = "task";
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        builder.Entity<Job>(e =>
        {
            e.ToTable("jobs", Schema);
            
            e.HasKey(x => x.Id);
        
            e.Property(x => x.CreatedAt);
            e.Property(x => x.UpdatedAt);
            // e.Property(x => x.IsRemoved);
            //
            // e.HasQueryFilter(x => !x.IsRemoved);
            //
            e.Property(x => x.Number);
            e.Property(x => x.User);
            e.Property(x => x.Status);
            e.Property(x => x.Closed);
            e.Property(x => x.Progress);
            e.Property(x => x.Result);
            e.Property(x => x.Arguments);
        });
    }
}