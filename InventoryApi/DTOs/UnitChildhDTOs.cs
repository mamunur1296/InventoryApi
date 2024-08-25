using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class UnitChildhDTOs : BaseDTOs
    {

        public string Name { get; set; }
        public string UnitMasterId { get; set; }

        public string UnitShortCode { get; set; }
        public string? DisplayName { get; set; }
        public string? UnitDescription { get; set; }
        //public UnitMaster UnitMaster { get; set; }

       // public ICollection<Product> Products { get; set; }
    }
}
