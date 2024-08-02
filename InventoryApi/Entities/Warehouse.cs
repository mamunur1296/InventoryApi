using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Warehouse : BaseEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
