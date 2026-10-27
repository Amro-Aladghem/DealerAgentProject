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

            modelBuilder.Entity<ChatRole>().HasData(
                new ChatRole { Id = 1, Name = "DealerAgent" },
                new ChatRole { Id = 2, Name = "AdvisorAgent" },
                new ChatRole { Id = 3, Name = "ProductsSearchAgent" },
                new ChatRole { Id = 4, Name = "User" },
                new ChatRole { Id = 5, Name = "Tool" }
            );

            modelBuilder.Entity<Notification>().Property(p => p.NotficationStatus)
                .HasConversion(
                    status => status.ToString(),
                    status => Enum.Parse<eNotificationStatus>(status)
                );

            modelBuilder.Entity<Notification>().Property(p => p.AgentType)
                .HasConversion(
                    agentType => agentType.ToString(),
                    agentType => Enum.Parse<eAgentType>(agentType)
                );

            modelBuilder.Entity<Chat>().Property(p => p.AgentType)
                .HasConversion<string>();
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
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<ChatRole> ChatRoles => Set<ChatRole>();
        public DbSet<Notification> Notifications => Set<Notification>();
    }
}
