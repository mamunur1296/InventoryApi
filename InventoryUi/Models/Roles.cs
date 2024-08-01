using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Roles : BaseModel
    {
        [DisplayName("Role")]
        public string RoleName { get; set; }
    }
}
