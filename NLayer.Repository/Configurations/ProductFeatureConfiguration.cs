using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nlayer.Core.Models;

namespace NLayer.Repository.Configurations;

public class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeature>
{
    public void Configure(EntityTypeBuilder<ProductFeature> builder)
    {
        builder.HasKey(x => x.ProductId);

        builder.Property(x => x.Color).IsRequired();

        builder
            .HasOne(x => x.Product)
            .WithOne(x => x.ProductFeature)
            .HasForeignKey<ProductFeature>(x => x.ProductId);
    }
    
}