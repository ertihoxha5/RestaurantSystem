namespace RestaurantSystem.Models.ViewModels
{
    public class SelectedRestaurantViewModel
    {
        public int SelectedRestaurantId { get; set; }
        public List<Restaurant> Restaurants { get; set; }
    }
}
