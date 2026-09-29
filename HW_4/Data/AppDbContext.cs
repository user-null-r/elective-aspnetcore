using HW_4.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_4.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();
        user.HasKey(item => item.Id);
        user.Property(item => item.UserName).HasMaxLength(50).IsRequired();
        user.Property(item => item.Email).HasMaxLength(254).IsRequired();
        user.Property(item => item.PasswordHash).IsRequired();
        user.HasIndex(item => item.UserName).IsUnique();
        user.HasIndex(item => item.Email).IsUnique();

        var product = modelBuilder.Entity<Product>();
        product.HasKey(item => item.Id);
        product.Property(item => item.Name).HasMaxLength(100).IsRequired();
        product.Property(item => item.Price).HasPrecision(18, 2);

        var order = modelBuilder.Entity<Order>();
        order.HasKey(item => item.Id);
        order.HasOne(item => item.User).WithMany(item => item.Orders)
            .HasForeignKey(item => item.UserId).OnDelete(DeleteBehavior.Cascade);
        order.HasOne(item => item.Product).WithMany(item => item.Orders)
            .HasForeignKey(item => item.ProductId).OnDelete(DeleteBehavior.Restrict);
        order.HasIndex(item => item.UserId);
    }
}

