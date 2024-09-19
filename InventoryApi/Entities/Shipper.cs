using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Shipper : BaseEntity
    {
        public string ShipperName { get; set; }
        public string Phone { get; set; }
        public ICollection<Order>? Orders { get; set; }

    }
}
