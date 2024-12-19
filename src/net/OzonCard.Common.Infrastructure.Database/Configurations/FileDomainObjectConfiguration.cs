using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonCard.Common.Domain.Files;
using OzonCard.Common.Infrastructure.Database.Configurations.Abstractions;

namespace OzonCard.Common.Infrastructure.Database.Configurations;

public class FileDomainObjectConfiguration : DomainObjectConfiguration<SaveFile>
{
    public override void Configure(EntityTypeBuilder<SaveFile> builder)
    {
        base.Configure(builder);
        builder.ToTable("files");
        builder.Property(x => x.Format);
        builder.Property(x => x.Name);
        builder.Property(x => x.User);
    }
}