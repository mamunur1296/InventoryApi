using System.Numerics;

namespace InventoryApi.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserImg { get; set; }
        public string? DeactiveBy { get; set; }
        public List<string> Roles { get; set; }
        public string? NID { get; set; }
        public string? Address { get; set; }
        public string? Job { get; set; }
        public string? About { get; set; }
        public string? Country { get; set; }
    }
}
