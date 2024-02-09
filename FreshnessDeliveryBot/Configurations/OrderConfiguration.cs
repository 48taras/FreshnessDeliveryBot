using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshnessDeliveryBot.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderID);

            builder.Property(x => x.OrderID).ValueGeneratedOnAdd();

            builder.Property(x => x.Status).HasMaxLength(100).IsRequired();

            builder.Property(x => x.OrderDate).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.HasMany(o => o.OrderItems).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderID).HasPrincipalKey(x => x.OrderID);
        }
    }
}
