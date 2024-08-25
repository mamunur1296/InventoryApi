using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Company : BaseEntity
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? FaxNo { get; set; }
        public string? EmailNo { get; set; }
        public bool IsActive { get; set; }
        public byte[]? Logo {  get; set; }
        public ICollection<Warehouse>? Warehouses { get; set; }

    }
}
