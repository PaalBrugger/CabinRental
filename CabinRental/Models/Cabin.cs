using System.ComponentModel.DataAnnotations;

namespace CabinRental.Models;

public class Cabin
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public string Description { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    
    // Navigation property for related images
    public ICollection<CabinImage> Images { get; set; } = new List<CabinImage>();
    
}