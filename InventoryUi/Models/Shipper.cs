using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Shipper : BaseModel
    {
        [Required]
        [DisplayName("Name")]
        public string ShipperName { get; set; }
        [DisplayName("Phone")]
        public string Phone { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
