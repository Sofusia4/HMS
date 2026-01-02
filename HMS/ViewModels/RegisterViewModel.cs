using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter a firstname")]
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Enter a lastname")]
        [Display(Name = "LastName")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Enter an email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Enter a phonenumber")]
        [Display(Name = "PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(3, ErrorMessage = "The password must be at least 3 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string? PasswordConfirm { get; set; }

        public string? Code { get; set; }
    }
}
