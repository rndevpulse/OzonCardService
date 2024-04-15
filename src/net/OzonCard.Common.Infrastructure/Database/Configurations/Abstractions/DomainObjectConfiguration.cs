using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzonCard.Common.Domain.Abstractions;

namespace OzonCard.Common.Infrastructure.Database.Configurations.Abstractions;

public abstract class DomainObjectConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : DomainObject
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.UpdatedAt);
        builder.Property(x => x.IsRemoved);

        builder.HasQueryFilter(x => !x.IsRemoved);
        
        builder.Property(x => x.Version)
            .IsConcurrencyToken()
            .IsRequired();
    }
}