using System.ComponentModel.DataAnnotations;

namespace RestaurantSystem.Models.ViewModels
    
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
