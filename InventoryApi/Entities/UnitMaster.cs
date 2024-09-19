using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class UnitMaster : BaseEntity
    {
        [Required(ErrorMessage = "Unit Master Name is required.")]
        public string Name { get; set; }

        public string? UnitMasterDescription { get; set; }

        public ICollection<Product>? Products { get; set; }
        public ICollection<UnitChild>? UnitChildrens { get; set; }
    }
}
