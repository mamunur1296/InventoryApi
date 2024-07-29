using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class OrderDTOs
    {
        public string Id { get; set; }
        public DateTime? OrderDate { get; set; }
        //public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
