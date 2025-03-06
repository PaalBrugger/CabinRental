using System.ComponentModel.DataAnnotations;

namespace CabinRental.ViewModels;

public class UserViewModel
{
    [Required] 
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool IsAdmin { get; set; }
}