using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using RestaurantSystem.Models;
using System.Linq;
using System.Transactions;
namespace RestaurantSystem.Services
{
    public class TableAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public TableAssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Table?> FindAvailableTable(int restaurantId, DateTime startTime,
            int numberOfGuests, int durationMinutes)
        {
            var endTime = startTime.AddMinutes(durationMinutes);
            using var transaction = await _context.Database.BeginTransactionAsync();

            var tables = await _context.Tables
                .Where(t => t.RestaurantId == restaurantId && t.Capacity >= numberOfGuests)
                .OrderBy(t => t.Capacity)
                .ToListAsync();


            foreach (var table in tables) {
                bool isTaken = await _context.Reservations.AnyAsync(r =>
                r.TableId == table.Id &&
                    r.StartTime < endTime &&
                    r.EndTime > startTime);

                if (!isTaken)
                {
                    
                    return table;
                }
            }
            
            return null;
        }
    }
}
