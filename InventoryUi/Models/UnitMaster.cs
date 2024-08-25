using System.ComponentModel;

namespace InventoryUi.Models
{
    public class UnitMaster : BaseModel
    {
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string? UnitMasterDescription { get; set; }

        // public ICollection<Product> Products { get; set; }
        //public ICollection<UnitChild> UnitChildrens { get; set; }
    }
}
