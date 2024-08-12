using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Payment : BaseModel
    {
        [DisplayName]
        public string OrderID { get; set; }
        public Order Order { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
