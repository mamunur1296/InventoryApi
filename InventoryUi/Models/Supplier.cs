using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Supplier : BaseModel
    {
        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(255, ErrorMessage = "Supplier name cannot be longer than 255 characters.")]
        [DisplayName("Name")]
        public string SupplierName { get; set; }

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

        [StringLength(255, ErrorMessage = "Fax number cannot be longer than 255 characters.")]
        [DisplayName("Fax")]
        public string Fax { get; set; }
        [DisplayName("HomePage")]

        public string HomePage { get; set; }
        [DisplayName("Products")]

        public ICollection<Product> Products { get; set; }
    }
}
