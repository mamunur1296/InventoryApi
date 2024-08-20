using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Product : BaseModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(255, ErrorMessage = "Product name cannot be longer than 255 characters.")]
        [DisplayName("Name")]
        public string ProductName { get; set; }
        [DisplayName("Description")]

        public string Description { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        [DisplayName("Category")]
        public string CategoryID { get; set; }

        [DisplayName("Category")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        [DisplayName("Supplier")]
        public string SupplierID { get; set; }

        [DisplayName("Supplier")]
        public Supplier Supplier { get; set; }

        [StringLength(50, ErrorMessage = "Quantity per unit cannot be longer than 50 characters.")]
        [DisplayName("Quantity Per Unit")]
        public string QuantityPerUnit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a non-negative value.")]
        [DisplayName("Price")]
        public decimal UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Units in stock must be a non-negative value.")]
        [DisplayName("Stock")]
        public int UnitsInStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be a non-negative value.")]
        [DisplayName("Reorder Level")]
        public int ReorderLevel { get; set; }
        [DisplayName("Discontinued")]
        public bool Discontinued { get; set; }

        [StringLength(255, ErrorMessage = "Batch number cannot be longer than 255 characters.")]
        [DisplayName("Batch Number")]
        public string BatchNumber { get; set; }
        [DisplayName("Expiration")]
        public DateTime? ExpirationDate { get; set; }
        [DisplayName("Image")]
        public string ImageURL { get; set; }
        [DisplayName("Image")]
        public List<IFormFile> Files { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a non-negative value.")]
        [DisplayName("Weight")]
        public decimal Weight { get; set; }

        [StringLength(255, ErrorMessage = "Dimensions cannot be longer than 255 characters.")]
        [DisplayName("Dimensions")]
        public string Dimensions { get; set; }
    }
}
