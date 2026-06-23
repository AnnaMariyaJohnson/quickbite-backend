using Microsoft.EntityFrameworkCore;
using QuickBite.Domain.Entities;

namespace QuickBite.Persistence.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext>options)
        : base(options)
        {
        }
    public DbSet<User> Users => Set<User>();
    public DbSet<Restaurant> Restaurants=>Set<Restaurant>();
    public DbSet<MenuItem> MenuItems=>Set<MenuItem>();
    public DbSet<Order> Orders { get; set; }
    public DbSet<Address> Addresses=>Set<Address>();
    public DbSet<Favorite> Favorites=>Set<Favorite>();
    public DbSet<OrderItem> OrderItems{get; set;}
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    public DbSet<Payment> Payments => Set<Payment>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

         modelBuilder.Entity<Order>()
            .Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.MenuItem)
            .WithMany()
            .HasForeignKey(oi => oi.MenuItemId);

        modelBuilder.Entity<Review>()
            .HasOne(r=>r.User)
            .WithMany()
            .HasForeignKey(r=>r.UserId);

        modelBuilder.Entity<Review>()
            .HasOne(r=>r.Restaurant)
            .WithMany()
            .HasForeignKey(r=>r.RestaurantId);

        modelBuilder.Entity<SupportTicket>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<Payment>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);
    }
}
