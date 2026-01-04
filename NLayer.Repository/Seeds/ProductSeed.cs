using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nlayer.Core.Models;

namespace NLayer.Repository.Seeds;

public class ProductSeed:IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasData(
            new Product { Id = 1, CategoryId = 1, Price = 90, Stock = 78, CreatedDate = new DateTime(2026, 1, 1), Name = "Faber Castel 0.7 Versatil Kalem",},
            new Product { Id = 2, CategoryId = 2, Price = 450, Stock = 12, CreatedDate = new DateTime(2026, 1, 2), Name = "Zengin Baba Fakir Baba",},
            new Product { Id = 3, CategoryId = 3, Price = 235, Stock = 36, CreatedDate = new DateTime(2026, 1, 3), Name = "A4 Kareli Defter 120 Yaprak",},
            new Product { Id = 4, CategoryId = 2, Price = 560, Stock = 3, CreatedDate = new DateTime(2026, 1, 15), Name = "Akıllı Yatırımcı",}
             );
    }
}