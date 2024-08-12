using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Warehouse : BaseModel
    {
        [Required(ErrorMessage = "Warehouse name is required.")]
        [StringLength(255, ErrorMessage = "Warehouse name cannot be longer than 255 characters.")]
        public string WarehouseName { get; set; }

        [StringLength(255, ErrorMessage = "Location cannot be longer than 255 characters.")]
        public string Location { get; set; }

        public ICollection<Stock> Stocks { get; set; }

    }
}
