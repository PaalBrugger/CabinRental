using CabinRental.Models;
using CabinRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabinRental.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private ApplicationDBContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    public AdminController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult ManageReservations()
    {
        var reservations = _context.Reservations.Include(r => r.User).OrderBy(r => r.Id).ToList();

        return View(reservations);
    }

    public IActionResult EditReservation(int id)
    {
        return View(_context.Reservations.Find(id));
    }

    public IActionResult AddReservation()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = _context.Reservations.Find(id);
        _context.Reservations.Remove(reservation);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Reservation deleted successfully!";

        return RedirectToAction("ManageReservations");
    }

    public IActionResult ManageUsers()
    {
        var users = _context.Users.ToList();

        return View(users);
    }

    public IActionResult CreateUserForm()
    {
        return View("CreateUserForm");
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("CreateUserForm");
        }

        var user = new IdentityUser { Email = model.Email, UserName = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)

        {
            if (model.IsAdmin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            TempData["SuccessMessage"] = "User created successfully!";
            return RedirectToAction("ManageUsers");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View("CreateUserForm", model);
    }

    public async Task<IActionResult> EditUserForm(string id)
    {
        var user = await _context.Users.FindAsync(id);
        var isAdmin =  await _userManager.IsInRoleAsync(user, "Admin");
        var userVM = new UserEditViewModel { Id = user.Id, Email = user.Email, IsAdmin = isAdmin};
        
        
        return View(userVM);
    }

    public IActionResult DeleteUser(string id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        TempData["SuccessMessage"] = "User deleted successfully!";

        return RedirectToAction("ManageUsers");
    }

    public IActionResult ManageCabins()
    {
        return View();
    }
}