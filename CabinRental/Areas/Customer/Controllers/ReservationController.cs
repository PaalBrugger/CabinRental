using CabinRental.Models;
using CabinRental.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabinRental.Areas.Customer.Controllers;
[Area("Customer")]
public class ReservationController : Controller
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ReservationController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> CreateReservationForm(int cabinId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            var returnUrl = Url.Page("/Reservation/CreateReservation", new { cabinId });

            return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
        }

        var cabin = _context.Cabins.Find(cabinId);

        if (cabin == null) return NotFound();

        var model = new ReservationViewModel
        {
            CabinId = cabin.Id,
            CabinName = cabin.Name,
            PricePerNight = cabin.Price,
            CheckInDate = DateTime.Today,
            CheckOutDate = DateTime.Today.AddDays(1)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(ReservationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var days = (model.CheckOutDate - model.CheckInDate).Days;
        var totalPrice = days * model.PricePerNight;

        var reservation = new Reservation
        {
            CabinId = model.CabinId,
            UserId = user.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Address = model.Address,
            CheckInDate = model.CheckInDate,
            CheckOutDate = model.CheckOutDate,
            TotalPrice = totalPrice
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return RedirectToAction("Confirmation", new { id = reservation.Id });
    }

    public IActionResult Confirmation(int id)
    {
        var reservation = _context.Reservations
            .Include(r => r.Cabin)
            .FirstOrDefault(r => r.Id == id);

        TempData["SuccessMessage"] = "Booking has been confirmed!";
        return View(reservation);
    }

    public async Task<IActionResult> Reservations()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return Unauthorized();
        }

        var reservations =
            await _context.Reservations.Include(r => r.Cabin)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();

        return View(reservations);
    }

    [HttpPost]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
        }

        TempData["SuccessMessage"] = "Reservation deleted successfully!";


        return RedirectToAction("Reservations");
    }

    public IActionResult ReservationDetails(int id)
    {
        var reservation = _context.Reservations.Include(r => r.Cabin)
            .FirstOrDefault(r => r.Id == id);
        
        return View(reservation);
    }
}