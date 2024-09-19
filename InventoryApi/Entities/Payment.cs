using InventoryApi.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Entities
{
    public class Payment : BaseEntity
    {
        public string OrderID { get; set; }
        public Order? Order { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }

    }
}
