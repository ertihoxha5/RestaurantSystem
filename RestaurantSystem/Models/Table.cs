using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RestaurantSystem.Models
{
    public class Table
    {
        public int Id { get; set; }

        public int TableNumber { get; set; }
        public int Capacity { get; set; }

        // Foreign Key
        public int RestaurantId { get; set; }

        // Navigation Property
        [ValidateNever]
        public Restaurant Restaurant { get; set; }

        public TableStatus Status { get; set; } = TableStatus.Available;

    }
}
