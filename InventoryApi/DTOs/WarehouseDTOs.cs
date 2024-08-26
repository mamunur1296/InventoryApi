using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class WarehouseDTOs : BaseDTOs
    {


        public string WarehouseName { get; set; }
        public string Location { get; set; }

        public string BranchId { get; set; }

        //[ForeignKey("BranchId")]
       // public Branch Branch { get; set; }
        //public ICollection<Stock> Stocks { get; set; }

    }
}
