using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class UnitChild : BaseEntity
    {
        [Required(ErrorMessage = "Unit Child Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Unit Master ID is required.")]
        public string UnitMasterId { get; set; }

        public string UnitShortCode { get; set; }
        public string? DisplayName { get; set; }
        public string? UnitDescription { get; set; }

        [ForeignKey("UnitMasterId")]
        public UnitMaster UnitMaster { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
