using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonCard.Common.Domain.Customers;
using OzonCard.Common.Infrastructure.Database.Configurations.Abstractions;

namespace OzonCard.Common.Infrastructure.Database.Configurations;

public class CustomerDomainObjectConfiguration : DomainObjectConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);
        builder.ToTable("customers");
        builder.Property(x => x.Name);
        builder.Property(x => x.BizId);
        builder.Property(x => x.OrgId);
        builder.Property(x => x.IsActive);
        
        builder.Property(x => x.TabNumber);
        builder.Property(x => x.Position);
        builder.Property(x => x.Division);
        builder.Property(x => x.Phone);
        builder.Property(x => x.Comment);

        builder.Ignore(x => x.Context);
        // builder.Property(x => x.LastVisit);

        builder.OwnsMany(x => x.Cards, cards =>
        {
            cards.ToTable("customers_cards");
            cards.Property(x => x.Track);
            cards.Property(x => x.Number);
            cards.Property(x => x.Created);
        });

        builder.OwnsMany(x => x.Wallets, wallets =>
        {
            wallets.ToTable("customers_wallets");
            wallets.Property(x => x.WalletId);
            wallets.Property(x => x.Balance);
            wallets.Property(x => x.Name);
            wallets.Property(x => x.ProgramType);
            wallets.Property(x => x.Type);
        });
    }
}