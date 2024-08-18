using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class ShoppingCartDTOs : BaseDTOs
    {
        
        public string CustomerID { get; set; }
        //public Customer Customer { get; set; }
        public DateTime CreatedDate { get; set; }
        //public ICollection<CartItem> ? CartItems { get; set; }

    }
}
