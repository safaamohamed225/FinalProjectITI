using System.ComponentModel.DataAnnotations;

namespace Waseet.DTO
{
    public class RegisterUserDTO
    {
        public string? ID_Identity { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 20 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Invalid phone number. Phone number must be 11 digits long and start with '01'")]
        public string Phone { get; set; }

        public string Role { get; set; }

        [Required(ErrorMessage = "SSN is required")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Invalid SSN. SSN must consist of exactly 14 digits.")]
        public string SSN { get; set; }

        public string Address { get; set; }
    }
}