using System.ComponentModel;

namespace InventoryUi.Models
{
    public class OrderProduct : BaseModel
    {
        public Order? Order { get; set; }

        [DisplayName("Product")]
        public string? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
