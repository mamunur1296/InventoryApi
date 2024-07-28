using System.ComponentModel;

namespace InventoryUi.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserImg { get; set; }
        public IFormFile? FormFile { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public string? NID { get; set; }
        public string? Address { get; set; }
        public string? Job { get; set; }
        public string? About { get; set; }
        public string? Country { get; set; }
        public List<string> Roles { get; set; }

    }
}
