namespace CabinRental.ViewModels;

using System.ComponentModel.DataAnnotations;

public class UserEditViewModel
{
    public string Id { get; set; }

    [Required] 
    public string Email { get; set; }

    [Required] 
    public string UserName { get; set; }

    public bool IsAdmin { get; set; }
}