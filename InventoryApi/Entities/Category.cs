using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Category : BaseEntity
    {
        public string? Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
