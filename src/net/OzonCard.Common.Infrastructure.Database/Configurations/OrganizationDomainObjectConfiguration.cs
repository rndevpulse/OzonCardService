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
        builder.Property(x => x.PaymentName);
        builder.Property(x => x.Name);
        builder.Property(x => x.Endpoint);
        builder.Property(x => x.Login);
        builder.Property(x => x.Password);
        builder.Property(x => x.Token);
        builder.Property(x => x.TransportId);
        
        builder.Ignore(x => x.CloudClient);

        builder.OwnsMany(x => x.Categories, categories =>
        {
            categories.ToTable("organizations_categories");
            categories.Property(x => x.Name);
            categories.Property(x => x.CategoryId);
            categories.Property(x => x.IsActive);
            
            builder.Property(typeof(int), "Id");
            builder.HasKey("Id");
        });
        
        builder.OwnsMany(x => x.Members, members =>
        {
            members.ToTable("organizations_members");
            members.Property(x => x.Name);
            members.Property(x => x.UserId);
            
            builder.Property(typeof(int), "Id");
            builder.HasKey("Id");
        });
        
        builder.OwnsMany(x => x.Programs, programs =>
        {
            programs.ToTable("organizations_programs");
            programs.Property(x => x.Name);
            programs.Property(x => x.IsActive);
            programs.HasKey(x => x.ProgramId);
            programs.HasKey(x => x.WalletId);
            programs.HasKey(x => x.WalletType);
            
            builder.Property(typeof(int), "Id");
            builder.HasKey("Id");
        });

    }
}