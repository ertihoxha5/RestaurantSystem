using Microsoft.AspNetCore.Identity;
using RestaurantSystem.Models.Enums;
namespace RestaurantSystem.Models

{

    public class Reservation
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int ClientProfileId { get; set; }
        public ClientProfile ClientProfile { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public int TableId { get; set; }
        public Table Table { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int NumberOfGuests { get; set; }
        public ReservationStatus Status { get; set; }

    }
}