using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Register 
    {
        [Required(ErrorMessage = "First Name is Required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "User Name is Required")]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid Email Format")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression(@"^(?:\+88|88)?(01[3-9]\d{8})$", ErrorMessage = "Invalid  Phone Number . Must be 11 digits.")]
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 12 characters.")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmationPassword { get; set; }
        public List<string> Roles { get; set; }
        [DisplayName("Roles")]
        public string RoleName { get; set; }
        public bool? isApproved { get; set; } = true;
        public bool? isEmployee { get; set; } = false;
        public bool? isApprovedByAdmin { get; set; } = false;
    }

}
