using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CabinRental.ViewModels;

public class AdminReservationViewModel
{
    [Required] 
    public string UserId { get; set; }
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


    [Required] 
    [DataType(DataType.Date)] public  DateTime CheckInDate { get; set; }

    [Required] 
    [DataType(DataType.Date)] public DateTime CheckOutDate { get; set; }

    public double PricePerNight { get; set; } // Preloaded from Cabin

    public double TotalPrice { get; set; } // Auto-calculated

    public List<SelectListItem> Users { get; set; } // List of users for dropdown
    public List<SelectListItem> Cabins { get; set; } // List of cabins for dropdown
}