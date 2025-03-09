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
        if (user == null) return NotFound();

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        var userViewModel = new UserEditViewModel { Id = user.Id, Email = user.Email, IsAdmin = isAdmin };


        return View(userViewModel);
    }

    public async Task<IActionResult> EditUser(UserEditViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null)
        {
            return NotFound();
        }

        user.Email = model.Email;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("EditUserForm", model);
        }

        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("EditUserForm", model);
            }
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (model.IsAdmin && !isAdmin)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
        else if (!model.IsAdmin && isAdmin)
        {
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        }

        return RedirectToAction("ManageUsers");
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
        var cabins = _context.Cabins.OrderBy(c => c.Id).ToList();

        return View(cabins);
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