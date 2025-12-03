using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Models;
using MyProject.Data;
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
    }
}
