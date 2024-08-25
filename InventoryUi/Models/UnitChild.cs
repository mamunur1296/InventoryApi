using System.ComponentModel;

namespace InventoryUi.Models
{
    public class UnitChild : BaseModel
    {
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Master Unit")]
        public string UnitMasterId { get; set; }
        [DisplayName("Code")]
        public string UnitShortCode { get; set; }
        [DisplayName("Display Name")]
        public string? DisplayName { get; set; }
        [DisplayName("Description")]
        public string? UnitDescription { get; set; }
        //public UnitMaster UnitMaster { get; set; }

        // public ICollection<Product> Products { get; set; }
    }
}
