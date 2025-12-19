using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Models;
using MyProject.Data;
using RestaurantSystem.Models.ViewModels;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class ClientProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ClientProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.ClientProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                profile = new ClientProfile { UserId = user.Id };
            }
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ClientProfile model)
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = await _context.ClientProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                profile = new ClientProfile
                {
                    UserId = user.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };
                _context.ClientProfiles.Add(profile);
            }
            else
            {
                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                profile.Address = model.Address;
                profile.PhoneNumber = model.PhoneNumber;
            }
            await _context.SaveChangesAsync();
            ViewBag.Message = "Profile updated successfully!";
            return View(profile);
        }

        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
                );

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);

                }
                return View(model);

            }
            TempData["Message"] = "Password changed successfully.";
            return RedirectToAction("Index");
        }
    }
}
