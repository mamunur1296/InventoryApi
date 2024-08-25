using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Branch : BaseModel
    {
        [DisplayName("Name")]
        public string? Name { get; set; }
        [DisplayName("Full Name")]
        public string? FullName { get; set; }
        [DisplayName("Contact Person")]
        public string? ContactPerson { get; set; }
        [DisplayName("Address")]
        public string? Address { get; set; }
        [DisplayName("Phone")]
        public string? PhoneNo { get; set; }
        [DisplayName("Fax")]
        public string? FaxNo { get; set; }
        [DisplayName("Email")]
        public string? EmailNo { get; set; }
        [DisplayName("Status")]
        public bool IsActive { get; set; }
        [DisplayName("Company")]
        public string CompanyId { get; set; }
        //public Company Company { get; set; }

        // public ICollection<Warehouse> Warehouses { get; set; }
    }
}
