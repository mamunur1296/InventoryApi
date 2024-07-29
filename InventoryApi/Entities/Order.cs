using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Order : BaseEntity
    {
        public DateTime? OrderDate { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
