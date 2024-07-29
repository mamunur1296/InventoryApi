using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Company : BaseEntity
    {

        public string? Name { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
