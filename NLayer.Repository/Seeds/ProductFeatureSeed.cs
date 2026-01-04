using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nlayer.Core.Models;

namespace NLayer.Repository.Seeds;

public class ProductFeatureSeed : IEntityTypeConfiguration<ProductFeature>
{
    public void Configure(EntityTypeBuilder<ProductFeature> builder)
    {
        builder.HasData(
            new ProductFeature { Id = 1,ProductId = 1, Color = "Kırmızı",Height = 90, Width = 50},
            new ProductFeature { Id = 2, ProductId = 2, Color = "Mavi",  Height = 90, Width = 50}
            );
    }
}