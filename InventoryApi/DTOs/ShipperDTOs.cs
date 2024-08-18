using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class ShipperDTOs : BaseDTOs
    {
        
        [Required]
        public string ShipperName { get; set; }
        public string Phone { get; set; }
       // public ICollection<Order> Orders { get; set; }

    }
}
