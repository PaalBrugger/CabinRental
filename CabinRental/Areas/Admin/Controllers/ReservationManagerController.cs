using CabinRental.Models;
using CabinRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabinRental.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class ReservationManagerController : Controller
{
    private ApplicationDBContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    public ReservationManagerController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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

}