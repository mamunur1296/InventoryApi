using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class CustomerDTOs
    {
        public string id { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(255, ErrorMessage = "Customer name cannot be longer than 255 characters.")]
        public string CustomerName { get; set; }

        [StringLength(255, ErrorMessage = "Contact name cannot be longer than 255 characters.")]
        public string ContactName { get; set; }

        [StringLength(255, ErrorMessage = "Contact title cannot be longer than 255 characters.")]
        public string ContactTitle { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot be longer than 255 characters.")]
        public string Address { get; set; }

        [StringLength(255, ErrorMessage = "City cannot be longer than 255 characters.")]
        public string City { get; set; }

        [StringLength(255, ErrorMessage = "Region cannot be longer than 255 characters.")]
        public string Region { get; set; }

        [StringLength(255, ErrorMessage = "Postal code cannot be longer than 255 characters.")]
        public string PostalCode { get; set; }

        [StringLength(255, ErrorMessage = "Country cannot be longer than 255 characters.")]
        public string Country { get; set; }

        [StringLength(255, ErrorMessage = "Phone number cannot be longer than 255 characters.")]
        public string Phone { get; set; }

        [StringLength(255, ErrorMessage = "Fax number cannot be longer than 255 characters.")]
        public string Fax { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot be longer than 255 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Password hash cannot be longer than 255 characters.")]
        public string PasswordHash { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string MedicalHistory { get; set; }

       // public ICollection<Order> Orders { get; set; }
       // public ICollection<Review> Reviews { get; set; }
       // public ICollection<ShoppingCart> ShoppingCarts { get; set; }

    }
}
