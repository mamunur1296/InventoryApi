using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class WarehouseDTOs
    {
        public string id { get; set; }
        public string? Location { get; set; }
        public string? CompanyId { get; set; }
       // public Company? Company { get; set; }
       // public ICollection<Product>? Products { get; set; }
    }
}
