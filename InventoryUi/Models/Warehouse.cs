using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Warehouse : BaseModel
    {
        [DisplayName("Company")]
        public string? CompanyId { get; set; }
        [DisplayName("Name")]
        public string? Name { get; set; }
        [DisplayName("Address")]
        public string? Address { get; set; }
        [DisplayName("Company")]
        public Company? Company { get; set; }
        //public ICollection<Product>? Products { get; set; }

    }
}
