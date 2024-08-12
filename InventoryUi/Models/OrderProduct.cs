namespace InventoryUi.Models
{
    public class OrderProduct : BaseModel
    {
        public Order? Order { get; set; }
        public string? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
