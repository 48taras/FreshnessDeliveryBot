using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;


namespace FreshnessDeliveryBot.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.ProductId).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(500).IsRequired();

            builder.Property(x => x.Price).IsRequired();

            builder.HasMany(p => p.OrderItems).WithOne(oi => oi.Product).HasForeignKey(oi => oi.ProductID).HasPrincipalKey(x => x.ProductId);

        }
    }
}
