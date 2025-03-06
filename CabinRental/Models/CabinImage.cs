using System.ComponentModel.DataAnnotations;

namespace CabinRental.Models;

public class CabinImage
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CabinId { get; set; } // Foreign key
    [Required]
    public string ImagePath { get; set; } // Store file path
    [Required]
    public Cabin Cabin { get; set; } // Navigation property
  
}