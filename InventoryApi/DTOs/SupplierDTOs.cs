using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class SupplierDTOs : BaseDTOs
    {


        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(255, ErrorMessage = "Supplier name cannot be longer than 255 characters.")]
        public string SupplierName { get; set; }

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

        public string HomePage { get; set; }

       // public ICollection<Product> Products { get; set; }

    }
}
