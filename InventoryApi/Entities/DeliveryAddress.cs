using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class DeliveryAddress : BaseEntity
    {
        public string UserId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Mobile { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeactivatedDate { get; set; }
        public string? DeactiveBy { get; set; }
        public bool? IsDefault { get; set; }
    }
}
