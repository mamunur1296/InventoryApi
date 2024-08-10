using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class ShoppingCart : BaseEntity
    {
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

    }
}
