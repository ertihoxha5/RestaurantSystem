using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Models.ViewModels;

namespace RestaurantSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser>_userManager;
        private readonly RoleManager<IdentityRole>_roleManager;
        public UsersController(
            UserManager<IdentityUser>userManager,
            RoleManager<IdentityRole>roleManager)
        {
            _userManager= userManager;
            _roleManager= roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users= _userManager.Users.ToList();
            var model= new List<UserListVM>();
            foreach(var user in users)
            {
                var roles= await _userManager.GetRolesAsync(user);
                model.Add(new UserListVM
                {
                    Id= user.Id,
                    Email= user.Email,
                    PhoneNumber= user.PhoneNumber,
                    Role= roles.FirstOrDefault() ?? "-"
                });
            }
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Roles= _roleManager.Roles.Select(r=>r.Name).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM model)
        {
            ViewBag.Roles=_roleManager.Roles.Select(r=>r.Name).ToList();
            if(!ModelState.IsValid)
                return View(model);
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if(existingUser != null)
            {
                ModelState.AddModelError("Email", "This email already exists");
                return View(model);
            }
            var existingPhone=_userManager.Users.FirstOrDefault(u=>u.PhoneNumber==model.PhoneNumber);
            if(existingPhone!= null)
            {
                ModelState.AddModelError("PhoneNumber", "This number already exists");
                return View(model);
            }
            var user=new IdentityUser
            {
                UserName= model.Email,
                Email= model.Email,
                PhoneNumber= model.PhoneNumber
            };

            var result= await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }
            await _userManager.AddToRoleAsync(user, model.Role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if(string.IsNullOrEmpty(id))
                return NotFound();
            var user= await _userManager.FindByIdAsync(id);
            if(user== null)
                return NotFound();
            var roles= await _userManager.GetRolesAsync(user);
            ViewBag.Roles= _roleManager.Roles.Select(r=> r.Name).ToList();
            return View(new EditUserVM
            {
                Id= user.Id,
                Email= user.Email,
                PhoneNumber= user.PhoneNumber,
                Role= roles.FirstOrDefault()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserVM model)
        {
            ViewBag.Roles = _roleManager.Roles.Select(r=> r.Name).ToList();
            if(!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByIdAsync(model.Id);
            if(user== null)
                return NotFound();
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            if(userWithSameEmail != null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This email is already used by another user");
                return View(model);
            }
            var userWithSamePhone = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber && u.Id != model.Id);
            if(userWithSamePhone != null)
            {
                ModelState.AddModelError("PhoneNumber", "This phone number is already used by another user");
                return View(model);
            }
            var oldRoles = await _userManager.GetRolesAsync(user);

            user.Email= model.Email;
            user.UserName= model.Email;
            user.PhoneNumber= model.PhoneNumber;

            await _userManager.UpdateAsync(user);
            await _userManager.RemoveFromRolesAsync(user, oldRoles);
            await _userManager.AddToRoleAsync(user, model.Role);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            if(string.IsNullOrEmpty(id))
                return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
                return NotFound();
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user= await _userManager.FindByIdAsync(id);
            if(user!= null)
                await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
