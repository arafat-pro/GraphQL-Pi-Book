using System.ComponentModel.DataAnnotations;

namespace Pi_Books.Data.ViewModels.Authentication
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(128, ErrorMessage = "Must be between 6 and 128 characters", MinimumLength = 6)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(64, ErrorMessage = "Must be between 8 and 64 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}