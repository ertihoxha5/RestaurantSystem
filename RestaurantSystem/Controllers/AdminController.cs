using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // DASHBOARD (ekzistues)
        public IActionResult Dashboard()
        {
            return View();
        }

        // =========================
        // ADMIN PROFILE - READ
        // =========================
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.AdminProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                profile = new AdminProfile
                {
                    UserId = user.Id
                };
            }

            return View(profile);
        }

        // =========================
        // ADMIN PROFILE - UPDATE
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(AdminProfile model)
        {
            var user = await _userManager.GetUserAsync(User);

            var profile = await _context.AdminProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                model.UserId = user.Id;
                _context.AdminProfiles.Add(model);
            }
            else
            {
                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                profile.PhoneNumber = model.PhoneNumber;

                _context.AdminProfiles.Update(profile);
            }

            await _context.SaveChangesAsync();

            ViewBag.Message = "Profile updated successfully";
            return View(profile ?? model);
        }
    }
}
