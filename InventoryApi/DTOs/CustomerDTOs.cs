using InventoryApi.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class CustomerDTOs : BaseDTOs
    {
      

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(255, ErrorMessage = "Customer name cannot be longer than 255 characters.")]
        public string? CustomerName { get; set; }

   
        public string? ContactName { get; set; }

   
        public string? ContactTitle { get; set; }

   
        public string? Address { get; set; }

      
        public string? City { get; set; }

  
        public string? Region { get; set; }


        public string? PostalCode { get; set; }


        public string? Country { get; set; }

 
        public string? Phone { get; set; }

        public string? Fax { get; set; }


        public string? Email { get; set; }

        public string? UserName { get; set; }

        public string ?Password { get; set; }

        public string? PasswordHash { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? MedicalHistory { get; set; }
        public string? UserId { get; set; }
        //public ApplicationUser? User { get; set; }

        // public ICollection<Order> Orders { get; set; }
        // public ICollection<Review> Reviews { get; set; }
        // public ICollection<ShoppingCart> ShoppingCarts { get; set; }


        public int totalAmount { get; set; }
        public int DiscountedAmount { get; set; }
        public int SubTotal { get; set; }
        public int vat { get; set; } = 0;
        public int PaymentAmount { get; set; }
        public int DueAmount { get; set; }
        public int FynalyPaymentAmount { get; set; }
        public int productDiscountedTotal { get; set; }


    }
}
