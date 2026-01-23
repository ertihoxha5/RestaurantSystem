namespace RestaurantSystem.Models.ViewModels
{
    public class AdminReservationViewModel
    {
        public List<Reservation> TodaysReservations { get; set; } = new();
        public List<Reservation> UpcomingReservations { get; set; } = new();
    }
}
