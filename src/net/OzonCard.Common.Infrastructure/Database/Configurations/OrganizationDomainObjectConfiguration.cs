using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonCard.Common.Domain.Organizations;
using OzonCard.Common.Infrastructure.Database.Configurations.Abstractions;

namespace OzonCard.Common.Infrastructure.Database.Configurations;

public class OrganizationDomainObjectConfiguration : DomainObjectConfiguration<Organization>
{
    public override void Configure(EntityTypeBuilder<Organization> builder)
    {
        base.Configure(builder);
        builder.ToTable("organizations");
        builder.Property(x => x.Name);
        builder.Property(x => x.Login);
        builder.Property(x => x.Password);

        builder.OwnsMany(x => x.Categories, categories =>
        {
            categories.ToTable("organizations_categories");
            categories.HasKey(x => x.Id);
            categories.Property(x => x.Name);
            categories.Property(x => x.IsActive);
        });
        
        builder.OwnsMany(x => x.Members, members =>
        {
            members.ToTable("organizations_members");
            members.Property(x => x.Name);
            members.Property(x => x.UserId);
        });
        
        builder.OwnsMany(x => x.Programs, programs =>
        {
            programs.ToTable("organizations_programs");
            programs.HasKey(x => x.Id);
            programs.Property(x => x.Name);
            programs.Property(x => x.IsActive);
            programs.OwnsMany(x => x.Wallets, wallets =>
            {
                wallets.ToTable("organizations_programs_wallets");
                wallets.HasKey(x => x.Id);
                wallets.Property(x => x.Name);
                wallets.Property(x => x.ProgramType);
                wallets.Property(x => x.Type);
            });

        });

    }
}