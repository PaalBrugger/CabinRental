using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CabinRental.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("Admin")]
public class DashboardController : Controller
{
    private ApplicationDBContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    public DashboardController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Dashboard()
    {
        return View();
    }
}