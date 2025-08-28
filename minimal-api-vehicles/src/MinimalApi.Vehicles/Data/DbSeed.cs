using Microsoft.EntityFrameworkCore;
using MinimalApi.Vehicles.Models;

namespace MinimalApi.Vehicles.Data;

public static class DbSeed
{
    public static async Task EnsureSeedDataAsync(this AppDbContext db)
    {
        if (await db.AdminUsers.AnyAsync()) return;

        var admin = new AdminUser
        {
            Email = "admin@local.test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin"
        };

        var editor = new AdminUser
        {
            Email = "editor@local.test",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Editor@123"),
            Role = "Editor"
        };

        db.AdminUsers.AddRange(admin, editor);
        await db.SaveChangesAsync();
    }
}
