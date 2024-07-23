using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonCard.Common.Domain.JobProgresses;

namespace OzonCard.Common.Infrastructure.Database.Configurations;

public class JobProgressEntityTypeConfiguration : IEntityTypeConfiguration<JobProgress>
{
    public void Configure(EntityTypeBuilder<JobProgress> builder)
    {
        builder.ToTable("job_progress");
        
        builder.Property(typeof(int), "Id");
        builder.HasKey("Id");
        
        builder.Property(x => x.TaskId).HasConversion<string>();
        builder.Property(x => x.Path);

        builder.HasIndex(x => x.TaskId);
    }
    
}