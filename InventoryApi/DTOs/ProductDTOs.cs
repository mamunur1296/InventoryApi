using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class ProductDTOs
    {
        public string id { get; set; }
        public string? Name { get; set; }
        public string? WarehouseId { get; set; }
       // public Warehouse? Warehouse { get; set; }
        public string? CategoryId { get; set; }
        //public CategoryDTOs? Category { get; set; }
        //public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
