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
    private readonly IWebHostEnvironment _hostEnvironment; 



    public CabinManagerController(ApplicationDBContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _hostEnvironment = hostEnvironment; 

        
    }
    public IActionResult ManageCabins()
    {
        var cabins = _context.Cabins.OrderBy(c => c.Id).ToList();

        return View(cabins);
    }

    public IActionResult CreateCabinForm()
    {
        return View();
    }
    
    public async Task<IActionResult> CreateCabin(AdminCreateCabinViewModel model)
    {
        Cabin newCabin = new Cabin {Name = model.Name, Description = model.Description, Address = model.Address, City = model.City, Price = model.PricePerNight};
        
        _context.Cabins.Add(newCabin);
        await _context.SaveChangesAsync();
        
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
                
                newCabin.Images.Add(new CabinImage{ ImagePath = relativePath, CabinId = newCabin.Id });
            }
        }
        _context.Update(newCabin);
        await _context.SaveChangesAsync();
        
        TempData["SuccessMessage"] = "Cabin Created Successfully!";

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
        
        TempData["SuccessMessage"] = "Cabin Updated Successfully!";

        
        return RedirectToAction("EditCabinForm", new { id = cabin.Id });
    }
    [HttpPost]
    public IActionResult DeleteCabinImage(int id)
    {
        var cabinImage = _context.CabinImages.FirstOrDefault(c => c.Id == id);
        if (cabinImage == null)
        {
            TempData["Error"] = "Image not found!";
            return RedirectToAction("ManageCabins");
        }
        string wwwRootPath = _hostEnvironment.WebRootPath;
        string imagePath = Path.Combine(wwwRootPath, cabinImage.ImagePath.TrimStart('/'));

        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath); 
        }
        _context.CabinImages.Remove(cabinImage);
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Cabin Updated Successfully!";

        return RedirectToAction("EditCabinForm", new { id = cabinImage.CabinId });
    }
    [HttpPost]
    public IActionResult DeleteCabin(int id)
    {
        var cabin = _context.Cabins.Find(id);
        if (cabin != null)
        {
            _context.Cabins.Remove(cabin);
            _context.SaveChanges();
        }

        TempData["SuccessMessage"] = "User Deleted Successfully!";

        return RedirectToAction("ManageCabins");
    }
}