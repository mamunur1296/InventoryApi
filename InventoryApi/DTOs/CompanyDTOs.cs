using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class CompanyDTOs  : BaseDTOs
    {

        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? FaxNo { get; set; }
        public string? EmailNo { get; set; }
        public bool IsActive { get; set; }
        //public Warehouse? Warehouse { get; set; }
    }
}
