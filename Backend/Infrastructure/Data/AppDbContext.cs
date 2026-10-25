using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var foreignKey in modelBuilder.Model
                                           .GetEntityTypes()
                                           .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<OrderItem>()
                .Property(p => p.ItemType)
                .HasConversion(
                    type => type.ToString(),
                    type => Enum.Parse<eOrderItemType>(type)
                );

            modelBuilder.Entity<CartItem>()
                .Property(p => p.ItemType)
                .HasConversion(
                    type => type.ToString(),
                    type => Enum.Parse<eOrderItemType>(type)
                );

            modelBuilder.Entity<Order>()
                .Property(p => p.Status)
                .HasConversion(
                    status => status.ToString(),
                    status => Enum.Parse<eOrderStatus>(status)
                );
        }

        public DbSet<Store> Stores => Set<Store>();
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<CarMake> CarMakes => Set<CarMake>();
        public DbSet<CarAccessory> CarAccessories => Set<CarAccessory>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Complaint> Complaints => Set<Complaint>();
    }
}
