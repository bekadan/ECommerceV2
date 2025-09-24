using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Configurations;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);  

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.Description);

        builder.OwnsOne(p => p.Price, price =>
        {
            price.WithOwner();

            price.Property(p => p.Amount)
                .IsRequired()
                .HasColumnName("PriceAmount");

            price.Property(p => p.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("PriceCurrency");
        });

        builder.Property(p => p.Stock)
            .IsRequired();

        builder.Property(p => p.CreatedOnUtc).HasColumnName("CreatedOnUtc").IsRequired();
        builder.Property(p => p.ModifiedOnUtc).HasColumnName("ModifiedOnUtc");
        builder.Property(p => p.Deleted).HasColumnName("Deleted");
        builder.Property(p=>p.Deleted).HasColumnName("Deleted").HasDefaultValue(false).IsRequired();
        builder.HasQueryFilter(p => !p.Deleted);
    }
}
