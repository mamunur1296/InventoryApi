using Microsoft.AspNetCore.Identity;


namespace InventoryApi.Entities
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserImg { get; set; }
        public string? NID { get; set; }
        public string ? Address { get; set; }
        public string ? Job { get; set; }
        public string? About { get; set; }
        public string? Country { get; set;}
        public bool? isApproved { get; set; } 
        public bool? isEmployee { get; set; } 
        public bool? isApprovedByAdmin { get; set; }
        public string? CompanyId { get; set; }
        public string? BranchId { get; set; }
    }
}
