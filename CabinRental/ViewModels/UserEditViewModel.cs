namespace CabinRental.ViewModels;

using System.ComponentModel.DataAnnotations;

public class UserEditViewModel
{
    public string Id { get; set; }

    [Required] 
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }
    public bool IsAdmin { get; set; }
}