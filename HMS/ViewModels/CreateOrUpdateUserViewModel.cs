using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class CreateOrUpdateUserViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^((\+)?\b(8|38)?(0[\d]{2}))([\d-]{5,8})([\d]{2})")]
        public string PhoneNumber { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string? PasswordConfirm { get; set; }
    }
}
