using System.ComponentModel.DataAnnotations;

namespace Pi_Books.Data.ViewModels.Authentication
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "User Name is Required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(64, ErrorMessage = "Must be between 8 and 64 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is Required!")]
        public string Role { get; set; }
    }
}