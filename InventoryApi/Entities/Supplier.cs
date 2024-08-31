using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Supplier : BaseEntity
    {
        public string SupplierName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string Phone { get; set; }
        public string ? Fax { get; set; }
        public string? HomePage { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
