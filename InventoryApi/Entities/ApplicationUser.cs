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
    }
}
