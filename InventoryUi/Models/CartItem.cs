﻿namespace InventoryUi.Models
{
    public class CartItem : BaseModel
    {
        public string CartID { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public string ProductID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}