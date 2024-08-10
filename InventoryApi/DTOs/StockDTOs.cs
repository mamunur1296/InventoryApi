using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class StockDTOs
    {
        public string id { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public string ProductID { get; set; }

        //public Product Product { get; set; }

        [Required(ErrorMessage = "Warehouse ID is required.")]
        public string WarehouseID { get; set; }

        //public Warehouse Warehouse { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; }
    }
}
