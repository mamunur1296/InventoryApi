using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class WarehouseDTOs
    {
        public string id { get; set; }
        public string? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }

        // public Company? Company { get; set; }
        // public ICollection<Product>? Products { get; set; }
    }
}
