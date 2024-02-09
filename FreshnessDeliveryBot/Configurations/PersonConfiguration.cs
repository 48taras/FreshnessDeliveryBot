using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshnessDeliveryBot.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.CustomerID);

            builder.Property(x => x.CustomerID).ValueGeneratedOnAdd();

            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Address).HasMaxLength(100).IsRequired();

            builder.Property(x => x.PhoneNumber).HasMaxLength(13).IsRequired();

            builder.HasMany(u => u.Orders).WithOne(o => o.person).HasForeignKey(o => o.PersonId).HasPrincipalKey(x => x.CustomerID);
        }
    }
}
