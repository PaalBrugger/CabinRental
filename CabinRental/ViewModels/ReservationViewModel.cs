using System.ComponentModel.DataAnnotations;

namespace CabinRental.ViewModels;

public class ReservationViewModel
{
    [Required]
    public int CabinId { get; set; }
    [Required]
    public string CabinName { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required] 
    public string PhoneNumber { get; set; }
    [Required]    
    public string Address { get; set; }

    [Required] [DataType(DataType.Date)] public DateTime CheckInDate { get; set; }

    [Required] [DataType(DataType.Date)] public DateTime CheckOutDate { get; set; }

    public double PricePerNight { get; set; } // Preloaded from Cabin
    
    public double TotalPrice { get; set; } // Calculated on submit
}