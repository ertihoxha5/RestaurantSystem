namespace RestaurantSystem.Models.ViewModels
{
    public class ReservationConfirmViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public int TableId { get; set; }
        public int TableNumber { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int NumberOfGuests { get; set; }
    }

}
