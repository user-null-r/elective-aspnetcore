using HW_3.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_3.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();
        user.HasKey(item => item.Id);
        user.Property(item => item.UserName).HasMaxLength(50).IsRequired();
        user.Property(item => item.Email).HasMaxLength(254).IsRequired();
        user.Property(item => item.PasswordHash).IsRequired();
        user.HasIndex(item => item.UserName).IsUnique();
        user.HasIndex(item => item.Email).IsUnique();
    }
}

