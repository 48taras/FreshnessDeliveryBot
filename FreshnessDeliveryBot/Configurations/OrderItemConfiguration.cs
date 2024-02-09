using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshnessDeliveryBot.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.OrderItemID);

            builder.Property(x => x.OrderItemID).ValueGeneratedOnAdd();

            builder.Property(x => x.Quantity).IsRequired();
        }
    }
}
