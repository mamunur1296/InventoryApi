using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InventoryUi.Models
{
    public class DeliveryAddress : BaseModel
    {
        [DisplayName("User Name")]
        public string UserId { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("Phone")]
        public string Phone { get; set; }
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
        [DisplayName("Status")]
        public bool IsActive { get; set; }
        [DisplayName("Deactivated Date")]
        public DateTime? DeactivatedDate { get; set; }
        [DisplayName("Deactive By")]
        public string? DeactiveBy { get; set; }
        [DisplayName("IsDefault")]
        public bool? IsDefault { get; set; }
    }
}
