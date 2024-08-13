using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Payment : BaseModel
    {
        [DisplayName("Order")]
        public string OrderID { get; set; }
        public Order Order { get; set; }
        [DisplayName("Date")]
        public DateTime PaymentDate { get; set; }
        [DisplayName("Method")]
        public string PaymentMethod { get; set; }
        [DisplayName("Amount")]
        public decimal Amount { get; set; }
        [DisplayName("Status")]
        public string PaymentStatus { get; set; }
    }
}
