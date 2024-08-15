using System.ComponentModel;

namespace InventoryUi.Models
{
    public class CartItem : BaseModel
    {
        [DisplayName("Cart Name")]
        public string CartID { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        [DisplayName("Product Name")]
        public string ProductID { get; set; }
        public Product Product { get; set; }
        [DisplayName("Quantity")]
        public int Quantity { get; set; }
    }
}
