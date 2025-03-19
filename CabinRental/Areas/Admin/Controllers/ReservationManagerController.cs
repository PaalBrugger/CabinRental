using CabinRental.Models;
using CabinRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public IActionResult EditReservationForm(int id)
    {
        var reservation = _context.Reservations.Include(r => r.User)
            .Include(r => r.Cabin)
            .FirstOrDefault(r => r.Id == id);

        if (reservation == null) return NotFound();

        return View(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> EditReservation(Reservation editedReservation)
    {
        var reservation = await _context.Reservations.FindAsync(editedReservation.Id);
        if (reservation == null) return NotFound();
        reservation.FirstName = editedReservation.FirstName;
        reservation.LastName = editedReservation.LastName;
        reservation.PhoneNumber = editedReservation.PhoneNumber;
        reservation.Address = editedReservation.Address;

        if (!editedReservation.CheckInDate.Equals(reservation.CheckInDate) ||
            !editedReservation.CheckOutDate.Equals(reservation.CheckOutDate))
        {
            bool isAvailable = await IsReservationAvailable(reservation.CabinId
                , editedReservation.CheckInDate
                , editedReservation.CheckOutDate, reservation.Id);

            if (!isAvailable)
            {
                TempData["Error"] = "Dates is already booked.";
                return RedirectToAction("EditReservationForm", new { id = reservation.Id });
            }

            reservation.CheckInDate = editedReservation.CheckInDate;
            reservation.CheckOutDate = editedReservation.CheckOutDate;

            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Reservation edited successfully!";
        }

        return RedirectToAction("ManageReservations");
    }

    public async Task<bool> IsReservationAvailable(int cabinId, DateTime checkIn, DateTime checkOut,
        int? reservationId = null)
    {
        bool hasConflict = await _context.Reservations
            .Where(r => r.CabinId == cabinId) // Ensure same cabin
            .Where(r => reservationId == null || r.Id != reservationId) // Exclude current reservation (if updating)
            .AnyAsync(r =>
                    (checkIn < r.CheckOutDate && checkOut > r.CheckInDate) // Check any overlap
            );

        return !hasConflict;
    }


    public async Task<IActionResult> CreateReservationForm()
    {
        var users = await _context.Users
            .Select(u => new SelectListItem { Value = u.Id, Text = u.Email })
            .ToListAsync();
        var cabins = await _context.Cabins.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToListAsync();
        
        var model = new AdminReservationViewModel{Users = users, Cabins = cabins, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now};
        
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> CreateReservation(AdminReservationViewModel model)
    {
        var days = (model.CheckOutDate - model.CheckInDate).Days;
        var cabin = _context.Cabins.FirstOrDefault(c => c.Id == model.CabinId);
        if (cabin == null) return NotFound();
        
        var totalPrice = days * cabin.Price;

        if (await IsReservationAvailable(model.CabinId, model.CheckInDate, model.CheckOutDate))
        {

            Reservation reservation = new Reservation
            {
                UserId = model.UserId, CheckInDate = model.CheckInDate, CheckOutDate = model.CheckOutDate,
                CabinId = model.CabinId, PhoneNumber = model.PhoneNumber, FirstName = model.FirstName,
                LastName = model.LastName, Address = model.Address, TotalPrice = totalPrice
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }
        else
        {
            TempData["Error"] = "Dates are booked.";
            return RedirectToAction("CreateReservationForm");
        }

        TempData["SuccessMessage"] = "Reservation added successfully!";
        return RedirectToAction("ManageReservations");
    }
    [HttpGet]
    public async Task<IActionResult> GetCabinDetails(int cabinId)
    {
        var cabin = await _context.Cabins
            .Where(c => c.Id == cabinId)
            .Select(c => new { PricePerNight = c.Price })
            .FirstOrDefaultAsync();

        if (cabin == null)
        {
            return NotFound();
        }

        return Json(cabin);
    }

    [HttpPost]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = _context.Reservations.Find(id);
        if (reservation == null) return NotFound();

        _context.Reservations.Remove(reservation);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Reservation deleted successfully!";

        return RedirectToAction("ManageReservations");
    }
}