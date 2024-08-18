using System.ComponentModel;

namespace InventoryUi.Models
{
    public class User : BaseModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }
        [DisplayName("Image")]
        public string? UserImg { get; set; }
        [DisplayName("Uplode Image")]
        public IFormFile? FormFile { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        public string ConfirmationPassword { get; set; }
        [DisplayName("NID")]
        public string? NID { get; set; }
        [DisplayName("Address")]
        public string? Address { get; set; }
        [DisplayName("Profession")]
        public string? Job { get; set; }
        [DisplayName("About")]
        public string? About { get; set; }
        [DisplayName("Cuntry")]
        public string? Country { get; set; }
        [DisplayName("Role")]
        public List<string> Roles { get; set; }
        [DisplayName("Role")]
        public string RoleName { get; set; }
    }
}
