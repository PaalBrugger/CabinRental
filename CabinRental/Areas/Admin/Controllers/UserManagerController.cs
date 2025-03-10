using CabinRental.Models;
using CabinRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabinRental.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("Admin")]
public class UserManagerController : Controller
{
    private ApplicationDBContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    public UserManagerController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
     public IActionResult ManageUsers()
    {
        var users = _context.Users.ToList();

        return View(users);
    }

    public IActionResult CreateUserForm()
    {
        return View();
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

}