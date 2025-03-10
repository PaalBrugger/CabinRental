using CabinRental.Models;
using CabinRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabinRental.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class CabinManagerController : Controller
{
    private ApplicationDBContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    public CabinManagerController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult ManageCabins()
    {
        var cabins = _context.Cabins.OrderBy(c => c.Id).ToList();

        return View(cabins);
    }

    public IActionResult CreateCabinForm()
    {
        return View("CreateCabinForm");
    }
    
    public IActionResult CreateCabin()
    {
        return RedirectToAction("ManageCabins");
    }

    public IActionResult EditCabinForm(int id)
    {
        var cabin = _context.Cabins.Include(c => c.Images).FirstOrDefault(c => c.Id == id);

        if (cabin == null) return NotFound();

        CabinViewModel cabinViewModel = new CabinViewModel
        {
            Id = cabin.Id, Name = cabin.Name, Description = cabin.Description, Address = cabin.Address,
            City = cabin.City, PricePerNight = cabin.Price, ExistingImages = cabin.Images,
        };

        return View(cabinViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditCabin(CabinViewModel model)
    {
        var cabin = _context.Cabins.FirstOrDefault(c => c.Id == model.Id);
        
        if (cabin == null) return NotFound();
        
        cabin.Name = model.Name;
        cabin.Description = model.Description;
        cabin.Price = model.PricePerNight;
        cabin.Address = model.Address;
        cabin.City = model.City;
        cabin.Price = model.PricePerNight;

        if (model.NewImages != null && model.NewImages.Any())
        {
            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Cabin");

            foreach (var file in model.NewImages)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string fullPath = Path.Combine(uploadDir, fileName);
                string relativePath = Path.Combine("/Images/Cabin", fileName); 


                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                
                cabin.Images.Add(new CabinImage{ ImagePath = relativePath, CabinId = cabin.Id });
            }
        }
        
        _context.Update(cabin);
        await _context.SaveChangesAsync();
        
        TempData["SuccessMessage"] = "Cabin Updated successfully!";

        
        return RedirectToAction("ManageCabins");
    }

    public IActionResult DeleteCabinImage(int id)
    {
        var cabinImage = _context.CabinImages.FirstOrDefault(c => c.CabinId == id);
        
        return View(cabinImage);
    }

    public IActionResult DeleteCabin(int id)
    {
        var cabin = _context.Cabins.Find(id);
        if (cabin != null)
        {
            _context.Cabins.Remove(cabin);
            _context.SaveChanges();
        }

        TempData["SuccessMessage"] = "User deleted successfully!";

        return RedirectToAction("ManageCabins");
    }
}