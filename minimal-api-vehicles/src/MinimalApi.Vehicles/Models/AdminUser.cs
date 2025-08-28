using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Vehicles.Models;

public class AdminUser
{
    public int Id { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    public string Role { get; set; } = "Editor"; // Admin ou Editor
}
