using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Payment : BaseEntity
    {
        public string OrderID { get; set; }
        public Order Order { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }

    }
}
