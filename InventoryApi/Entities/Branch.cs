using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Branch : BaseEntity
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? FaxNo { get; set; }
        public string? EmailNo { get; set; }
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "Company ID is required.")]
        public string CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company ?Company { get; set; }

        public ICollection<Warehouse> ?Warehouses { get; set; }
    }
}
