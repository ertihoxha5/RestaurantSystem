using System.ComponentModel.DataAnnotations;
namespace RestaurantSystem.Models.ViewModels
{
    public class CreateUserVM
    {
        [Required(ErrorMessage = "Email address is mandatory")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Phone number is not valid")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Phone number must be between 7 and 12 digits")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please select a role")]
        public string Role { get; set; }
    }
}
