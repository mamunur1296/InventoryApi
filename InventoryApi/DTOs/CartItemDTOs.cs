using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class CartItemDTOs
    {
        public string id { get; set; }
        public string CartID { get; set; }
        //public ShoppingCart ShoppingCart { get; set; }
        public string ProductID { get; set; }
        //public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
