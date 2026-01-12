using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using RestaurantSystem.Models;

namespace RestaurantSystem.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index(string search)
        {
            var restaurants = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                restaurants = restaurants.Where(r =>
                    r.Name.Contains(search) || r.Location.Contains(search));
            }

            ViewBag.Search = search;

            return View(await restaurants.ToListAsync());
        }


        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Description")] Restaurant restaurant)
        {
            restaurant.CreatedAt = DateTime.Now;
            restaurant.UpdatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(restaurant);
                    await _context.SaveChangesAsync();

                    // Success alert
                    TempData["Success"] = "Restaurant created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Error alert
                    TempData["Error"] = "Failed to create restaurant. " + ex.Message;
                    return RedirectToAction(nameof(Index));
                }
            }

            // Validation errors
            TempData["Error"] = "Please check the form for errors.";
            return View(restaurant);
        }


        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Description,CreatedAt,UpdatedAt")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                TempData["Error"] = "Restaurant not found.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    restaurant.UpdatedAt = DateTime.Now;
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Restaurant updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
                    {
                        TempData["Error"] = "Restaurant no longer exists.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Failed to update restaurant. Please try again.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            // If validation fails, stay on edit page
            TempData["Error"] = "Please check the form for errors.";
            return View(restaurant);
        }


        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                try
                {
                    _context.Restaurants.Remove(restaurant);
                    await _context.SaveChangesAsync();

                    // Success alert
                    TempData["Success"] = "Restaurant deleted successfully!";
                }
                catch (Exception ex)
                {
                    // Error alert
                    TempData["Error"] = "Failed to delete restaurant. " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Restaurant not found.";
            }

            return RedirectToAction(nameof(Index));
        }


        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
