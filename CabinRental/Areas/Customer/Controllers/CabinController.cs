using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabinRental.Areas.Customer.Controllers;

[Area("Customer")]
public class CabinController : Controller
{
    private readonly ILogger<CabinController> _logger;
    private ApplicationDBContext _context;

    public CabinController(ILogger<CabinController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult CabinList()
    {
        var cabins = _context.Cabins.Include(c => c.Images).ToList();
        return View("CabinList",cabins);
    }

    public IActionResult Cabin(int? cabinId)
    {
        var cabin = _context.Cabins.Include(c => c.Images).Include(c => c.Reservations)
            .FirstOrDefault(c => c.Id == cabinId);

        if (cabin == null)
        {
            return NotFound();
        }

        return View(cabin);
    }

    // API Endpoint to Get Reservations for Calendar
    public async Task<IActionResult> GetReservations(int cabinId)
    {
        var reservations = await _context.Reservations
            .Where(r => r.CabinId == cabinId)
            .Select(r => new
            {
                title = "Booked",
                start = r.CheckInDate.ToString("yyyy-MM-dd"),
                end = r.CheckOutDate.AddDays(1).ToString("yyyy-MM-dd"), // Add 1 day to cover full booking
                Color = "red", // Unavailable days in red
                allDay = true
            })
            .ToListAsync();

        return Json(reservations);
    }
}