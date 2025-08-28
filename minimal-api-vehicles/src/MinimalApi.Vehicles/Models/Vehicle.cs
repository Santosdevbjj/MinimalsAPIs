using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Vehicles.Models;

public class Vehicle
{
    public int Id { get; set; }

    [Required]
    public string Make { get; set; } = null!;

    [Required]
    public string Model { get; set; } = null!;

    [Required]
    [MaxLength(17)]
    public string Vin { get; set; } = null!;

    public int Year { get; set; }
}
