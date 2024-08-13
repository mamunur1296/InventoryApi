using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Stock : BaseModel
    {
        [Required(ErrorMessage = "Product ID is required.")]
        [DisplayName("Product")]
        public string ProductID { get; set; }

        [ForeignKey("ProductID")]
        [DisplayName("Product")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Warehouse ID is required.")]
        [DisplayName("Warehouse")]
        public string WarehouseID { get; set; }

        [ForeignKey("WarehouseID")]
        [DisplayName("Warehouse")]
        public Warehouse Warehouse { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        [DisplayName("Quantity")]
        public int Quantity { get; set; }
    }
}
