using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Product : BaseModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(255, ErrorMessage = "Product name cannot be longer than 255 characters.")]
        public string ProductName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public string CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        public string SupplierID { get; set; }

        [ForeignKey("SupplierID")]
        public Supplier Supplier { get; set; }

        [StringLength(50, ErrorMessage = "Quantity per unit cannot be longer than 50 characters.")]
        public string QuantityPerUnit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a non-negative value.")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Units in stock must be a non-negative value.")]
        public int UnitsInStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be a non-negative value.")]
        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [StringLength(255, ErrorMessage = "Batch number cannot be longer than 255 characters.")]
        public string BatchNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string ImageURL { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a non-negative value.")]
        public decimal Weight { get; set; }

        [StringLength(255, ErrorMessage = "Dimensions cannot be longer than 255 characters.")]
        public string Dimensions { get; set; }
    }
}
