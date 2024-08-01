using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Company : BaseModel
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
       
        public bool IsActive { get; set; }
        public Warehouse? Warehouse { get; set; }

    }
}
