using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models.ViewModels
{
    public class ReservationDetailsViewModel
    {
        public int RestaurantId { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }

        [Required]
        [Range(1, 20)]
        public int NumberOfGuests {  get; set; }
    }
}
