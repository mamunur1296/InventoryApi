using InventoryUi.DTOs;
using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class ProfileViewModel
    {
        

        public User User { get; set; } = new User();
        
        public ChangePassword ChangePassword { get; set; } = new ChangePassword();

    }
}
