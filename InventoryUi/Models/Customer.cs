using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Customer : BaseModel
    {
        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(255, ErrorMessage = "Customer name cannot be longer than 255 characters.")]
        [DisplayName("Name")]
        public string CustomerName { get; set; }

        [StringLength(255, ErrorMessage = "Contact name cannot be longer than 255 characters.")]
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [StringLength(255, ErrorMessage = "Contact title cannot be longer than 255 characters.")]
        [DisplayName("Contact Title")]
        public string ContactTitle { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot be longer than 255 characters.")]
        [DisplayName("Address")]
        public string Address { get; set; }


        [StringLength(255, ErrorMessage = "City cannot be longer than 255 characters.")]
        [DisplayName("City")]
        public string City { get; set; }

        [StringLength(255, ErrorMessage = "Region cannot be longer than 255 characters.")]
        [DisplayName("Region")]
        public string Region { get; set; }

        [StringLength(255, ErrorMessage = "Postal code cannot be longer than 255 characters.")]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(255, ErrorMessage = "Country cannot be longer than 255 characters.")]
        [DisplayName("Country")]
        public string Country { get; set; }

        [StringLength(255, ErrorMessage = "Phone number cannot be longer than 255 characters.")]
        [DisplayName("Phone")]
        public string Phone { get; set; }


        [DisplayName("Fax")]
        public string ? Fax { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot be longer than 255 characters.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Password hash cannot be longer than 255 characters.")]
        [DisplayName("Password")]
        public string PasswordHash { get; set; }
        [DisplayName("Birth Date")]
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Medical History")]
        public string MedicalHistory { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
