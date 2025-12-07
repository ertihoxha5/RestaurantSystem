
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
 
[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // You can pass data from your models to the view if needed
            return View();
        }
    }
}
