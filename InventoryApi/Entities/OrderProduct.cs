using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class OrderProduct : BaseEntity
    {
        public Order? Order { get; set; }
        public string ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
