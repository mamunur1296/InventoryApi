using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Warehouse : BaseEntity
    {
        public string WarehouseName { get; set; }
        public string Location { get; set; }
        public string? CompanyId { get; set; }
        public Company? Company { get; set; }
        public ICollection<Stock> Stocks { get; set; }

    }
}
