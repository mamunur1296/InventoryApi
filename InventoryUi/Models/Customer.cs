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


        [DisplayName("Contact Name")]
        public string? ContactName { get; set; }


        [DisplayName("Contact Title")]
        public string? ContactTitle { get; set; }

        [DisplayName("Address")]
        public string? Address { get; set; }



        [DisplayName("City")]
        public string? City { get; set; }


        [DisplayName("Region")]
        public string? Region { get; set; }


        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        [DisplayName("Country")]
        public string? Country { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }


        [DisplayName("Fax")]
        public string ? Fax { get; set; }


        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Password hash cannot be longer than 255 characters.")]
        [DisplayName("Password")]
        public string PasswordHash { get; set; }
        [DisplayName("Birth Date")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("Medical History")]
        public string? MedicalHistory { get; set; }
        [DisplayName("User")]
        public string? UserId { get; set; }

        public int totalAmount { get; set; }
        public int DiscountedAmount { get; set; }
        public int SubTotal { get; set;  }
        public int vat { get; set; } = 0;
        public int PaymentAmount { get; set; } 
        public int DueAmount { get; set; } 
        public int FynalyPaymentAmount { get; set; } 
        public int productDiscountedTotal { get; set; } 


        public User? User { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
