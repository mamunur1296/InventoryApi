using InventoryApi.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Entities
{
    public class OrderDetail : BaseEntity
    {
        public string OrderID { get; set; }
        public Order Order { get; set; }
        public string ProductID { get; set; }
        public Product Product { get; set; }
        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 2)]
        public decimal Discount { get; set; }

    }
}
