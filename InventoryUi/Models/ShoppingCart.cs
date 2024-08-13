using System.ComponentModel;

namespace InventoryUi.Models
{
    public class ShoppingCart : BaseModel
    {
        [DisplayName("Customer")]
        public string CustomerID { get; set; }
        [DisplayName("Customer")]
        public Customer Customer { get; set; }
        [DisplayName("date")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Cart Items")]
        public ICollection<CartItem> CartItems { get; set; }
    }
}
