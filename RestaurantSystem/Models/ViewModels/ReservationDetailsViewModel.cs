using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models.ViewModels
{
    public class ReservationDetailsViewModel
    {
        public int RestaurantId { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Number of guests must be at least 1.")]
        public int NumberOfGuests {  get; set; }
    }
}
