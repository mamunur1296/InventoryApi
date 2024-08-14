using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Customer : BaseEntity
    {

        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ? Fax { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string MedicalHistory { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

    }
}
