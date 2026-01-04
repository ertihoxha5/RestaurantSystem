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
    public class TablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tables
        public async Task<IActionResult> Index(int? restaurantId)
        {
            IQueryable<Table> tables = _context.Tables.Include(t => t.Restaurant);

            if (restaurantId.HasValue)
            {
                tables = tables.Where(t => t.RestaurantId == restaurantId.Value);

                var restaurant = await _context.Restaurants.FindAsync(restaurantId.Value);
                ViewBag.RestaurantName = restaurant?.Name;
                ViewBag.RestaurantId = restaurantId.Value;
            }

            return View(await tables.ToListAsync());
        }



        // GET: Tables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables
                .Include(t => t.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // GET: Tables/Create
        public IActionResult Create(int restaurantId)
        {
            if (restaurantId == 0)
                return BadRequest("RestaurantId missing");

            var model = new Table
            {
                RestaurantId = restaurantId,
                Status = TableStatus.Available
            };

            return View(model);
        }



        // POST: Tables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Table table)
        {
            bool tableExists = await _context.Tables.AnyAsync(t =>
    t.RestaurantId == table.RestaurantId &&
    t.TableNumber == table.TableNumber
);

            if (tableExists)
            {
                ModelState.AddModelError("TableNumber",
                    "This table number already exists for this restaurant.");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();

                // TEMP: See errors
                ViewBag.Errors = errors;

                return View(table);
            }

            _context.Add(table);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { restaurantId = table.RestaurantId });
        }


        // GET: Tables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.StatusList = Enum.GetValues(typeof(TableStatus))
                        .Cast<TableStatus>()
                        .Select(s => new SelectListItem
                        {
                            Value = s.ToString(),
                            Text = s.ToString()
                        })
                        .ToList();

            var table = await _context.Tables
                .Include(t => t.Restaurant)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (table == null)
            {
                return NotFound();
            }

            ViewBag.RestaurantName = table.Restaurant.Name;

            return View(table);
        }


        // POST: Tables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TableNumber,Capacity,RestaurantId,Status")] Table table)
        {
            if (id != table.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { restaurantId = table.RestaurantId });
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurants, "Id", "Location", table.RestaurantId);
            return View(table);
        }

        // GET: Tables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables
                .Include(t => t.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (table == null)
            {
                return NotFound();
            }

            // Add restaurant name for the view
            ViewBag.RestaurantName = table.Restaurant?.Name;

            return View(table);
        }


        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table != null)
            {
                _context.Tables.Remove(table);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(int id)
        {
            return _context.Tables.Any(e => e.Id == id);
        }
    }
}
