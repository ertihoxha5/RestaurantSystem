using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using MyProject.Data;
using RestaurantSystem.Migrations;
using RestaurantSystem.Models;
using RestaurantSystem.Models.Enums;
using RestaurantSystem.Models.ViewModels;
using RestaurantSystem.Services;
using System.Security.Claims;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TableAssignmentService _tableAssignmentService;

        public ReservationController(ApplicationDbContext context, TableAssignmentService tableAssignmentService) 
        {
            _context = context;
            _tableAssignmentService= tableAssignmentService;
        }

        public async Task<IActionResult> Create()
        {
            var vm = new SelectedRestaurantViewModel
            {
                Restaurants = await _context.Restaurants.ToListAsync()
            };
            return View("SelectRestaurant",vm);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int restaurantId)
        {
            var restaurantExists = await _context.Restaurants.AnyAsync(r => r.Id == restaurantId);
            if (!restaurantExists)
            {
                return NotFound();
            }

            return View(new ReservationDetailsViewModel
            {
                RestaurantId = restaurantId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Details(ReservationDetailsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            
            


            var restaurant = await _context.Restaurants.FindAsync(vm.RestaurantId);
            if (restaurant == null)
            {
                return NotFound();
            }
            var startTime = vm.StartTime!.Value;
            var duration = restaurant.ReservationDurationTime;
            var endTime = startTime.AddMinutes(duration);


            var table = await _tableAssignmentService.FindAvailableTable(
                vm.RestaurantId,
                startTime,
                vm.NumberOfGuests,
                duration);

            if(table == null)
            {
                ModelState.AddModelError("", "No available tables.");
                return View(vm);
            }
            var confirmVm = new ReservationConfirmViewModel
            {
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                TableId = table.Id,
                TableNumber = table.TableNumber ?? table.Id,
                StartTime = startTime,
                EndTime = endTime,
                NumberOfGuests = vm.NumberOfGuests
            };

            return View("Confirm", confirmVm);
            

        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Confirm(ReservationConfirmViewModel vm)
        {
            var tableExists = await _context.Tables.AnyAsync(t => t.Id == vm.TableId);
            if (!tableExists)
            {
                ModelState.AddModelError("", "Selected table is no longer available.");
                return RedirectToAction("Details", new { restaurantId = vm.RestaurantId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var clientProfile = await _context.ClientProfiles
    .FirstOrDefaultAsync(cp => cp.UserId == userId);

            if (clientProfile == null)
            {

                ModelState.AddModelError("", "Client profile not found.");
                return View(vm);
            }
            var reservation = new Reservation
            {
                UserId = userId,
                ClientProfileId = clientProfile.Id,
                RestaurantId = vm.RestaurantId,
                TableId = vm.TableId,
                StartTime = vm.StartTime,
                EndTime  =vm.EndTime,
                NumberOfGuests = vm.NumberOfGuests,
                Status = ReservationStatus.Confirmed
            };
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success", new { id = reservation.Id });
           
        }
        public async Task<IActionResult> Success(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Restaurant)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

    }
}
