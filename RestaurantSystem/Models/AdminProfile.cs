using Microsoft.AspNetCore.Identity;

namespace RestaurantSystem.Models
{
    public class AdminProfile
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}