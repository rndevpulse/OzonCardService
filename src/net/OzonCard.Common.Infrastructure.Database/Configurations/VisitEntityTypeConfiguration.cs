using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonCard.Common.Domain.Customers;

namespace OzonCard.Common.Infrastructure.Database.Configurations;

public class VisitEntityTypeConfiguration : IEntityTypeConfiguration<CustomerVisit>
{
    public void Configure(EntityTypeBuilder<CustomerVisit> builder)
    {
        builder.ToTable("customers_visits");
        builder.Property(x => x.Customer);
        builder.Property(x => x.Date);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.Sum);

        builder.Property(typeof(int), "Id");
        builder.HasKey("Id");

        builder.HasIndex(x => x.Customer);
    }
}