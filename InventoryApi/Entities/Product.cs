using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
