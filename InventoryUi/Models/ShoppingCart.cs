namespace InventoryUi.Models
{
    public class ShoppingCart : BaseModel
    {
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
