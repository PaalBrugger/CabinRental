using CabinRental.Models;

namespace CabinRental.ViewModels;

using System.ComponentModel.DataAnnotations;

public class AdminCreateCabinViewModel
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string Description { get; set; }

    
    [Required]
    [Range(1, 10000, ErrorMessage = "Price must be greater than 0.")]
    public double PricePerNight { get; set; }
    
    public ICollection<IFormFile>? NewImages { get; set; }  //  Allows uploading new images

    


    
}