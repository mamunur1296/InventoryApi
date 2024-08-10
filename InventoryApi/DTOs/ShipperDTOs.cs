using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class ShipperDTOs
    {
        public string id { get; set; }
        [Required]
        public string ShipperName { get; set; }
        public string Phone { get; set; }
       // public ICollection<Order> Orders { get; set; }

    }
}
