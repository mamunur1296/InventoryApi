using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Stock : BaseEntity
    {
        public string ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        public string WarehouseID { get; set; }

        [ForeignKey("WarehouseID")]
        public Warehouse Warehouse { get; set; }
        public int Quantity { get; set; }

    }
}
