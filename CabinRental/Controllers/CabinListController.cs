using System.Diagnostics;
using CabinRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabinRental.Controllers;

public class CabinListController : Controller
{
    private readonly ILogger<CabinListController> _logger;
    private ApplicationDBContext _context;

    public CabinListController(ILogger<CabinListController> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult CabinList()
    {
        var cabins = _context.Cabins.Include(c => c.Images).ToList();
        return View(cabins);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}