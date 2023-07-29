using InvSys.Application.Entities;
using InvSys.Application.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvSys.Application.Infrastructure.Persistence.Configurations;

public class ProductEntityConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                    id => id.Value,
                    value => ProductId.Create(value));

        builder.Property(p => p.Name).HasMaxLength(50);

        // builder.Property(p => p.SKU)
        //     .HasConversion(
        //             sku => sku.Value,
        //             value => SKU.Create(value));
        builder.OwnsOne(p => p.SKU);

        builder.Property(p => p.Condition).HasMaxLength(20);

        // builder.OwnsOne(p => p.Location);

        builder.OwnsOne(p => p.AvailableQuantity);

        builder.OwnsOne(p => p.StockQuantity);

        builder.OwnsOne(p => p.Price);


    }
}