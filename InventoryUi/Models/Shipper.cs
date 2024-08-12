using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Shipper : BaseModel
    {
        [Required]
        public string ShipperName { get; set; }
        public string Phone { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
