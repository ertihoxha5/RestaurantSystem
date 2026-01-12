using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models
{
    public class Table
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Table number is required")]
        public int? TableNumber { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        public int? Capacity { get; set; }

        // Foreign Key
        public int RestaurantId { get; set; }

        // Navigation Property
        [ValidateNever]
        public Restaurant Restaurant { get; set; }

        public TableStatus Status { get; set; } = TableStatus.Available;

    }
}
