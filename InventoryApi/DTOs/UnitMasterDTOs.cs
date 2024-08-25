using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class UnitMasterDTOs : BaseDTOs
    {
        public string Name { get; set; }

        public string? UnitMasterDescription { get; set; }

       // public ICollection<Product> Products { get; set; }
        //public ICollection<UnitChild> UnitChildrens { get; set; }
    }
}
