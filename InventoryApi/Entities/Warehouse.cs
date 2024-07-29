using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Warehouse : BaseEntity
    {
        public string? Location { get; set; }
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
