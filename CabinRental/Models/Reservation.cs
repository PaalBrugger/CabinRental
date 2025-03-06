using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CabinRental.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CabinId { get; set; } // Foreign key to Cabin

    [Required]
    public string UserId { get; set; } // Foreign key to User 
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required] 
    public string PhoneNumber { get; set; } 
    
    [Required]
    public string Address { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public double TotalPrice { get; set; } 

    [ForeignKey("CabinId")]
    public Cabin Cabin { get; set; }    // Link to cabin

    [ForeignKey("UserId")]
    public IdentityUser User { get; set; } // Link to Identity user
}