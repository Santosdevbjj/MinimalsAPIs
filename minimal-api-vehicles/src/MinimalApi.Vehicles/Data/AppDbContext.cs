using Microsoft.EntityFrameworkCore;
using MinimalApi.Vehicles.Models;

namespace MinimalApi.Vehicles.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AdminUser>().HasIndex(a => a.Email).IsUnique();
    }
}
