using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonCard.Common.Domain.Props;
using OzonCard.Common.Infrastructure.Database.Configurations.Abstractions;

namespace OzonCard.Common.Infrastructure.Database.Configurations;

public class PropertyDomainObjectConfiguration : DomainObjectConfiguration<Property>
{
    public override void Configure(EntityTypeBuilder<Property> builder)
    {
        base.Configure(builder);
        builder.ToTable("properties");
        
        builder.Property(x => x.Reference);
        builder.Property(x => x.Type);
        builder.Property(x => x.Data);

        // builder.HasIndex(x => new { x.Type, x.Reference }).IsUnique();

        builder.HasDiscriminator(x => x.Type)
            .HasValue<MemberReportBatchProp>(PropType.MemberBatch)
            .IsComplete();
    }
    
    public void Configure(EntityTypeBuilder<MemberReportBatchProp> builder)
    {
        builder.Property(x => x.Organization);
        builder.Property(x => x.Name);
    }

    
}