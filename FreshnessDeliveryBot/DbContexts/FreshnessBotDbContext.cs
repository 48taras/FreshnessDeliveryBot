using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshnessDeliveryBot.Configurations;
using FreshnessDeliveryBot.Models;
using Microsoft.EntityFrameworkCore;

namespace FreshnessDeliveryBot.DbContexts
{
    public class FreshnessBotDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        public FreshnessBotDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());

            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\FreshnessDeliveryBot\\FreshnessDeliveryBot\\freshnessDeliveryBot.db");
        }

    }
}
