using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Warehouse : BaseEntity
    {
        public string WarehouseName { get; set; }
        public string Location { get; set; }

        public string BranchId { get; set; }

        //[ForeignKey("BranchId")]
        public Branch Branch { get; set; }
        public ICollection<Stock> Stocks { get; set; }

    }
}
