using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class DashboirdUserVm
    {
        public IEnumerable<Company> Companies=new List<Company>();
        public User User { get; set; }
        public IEnumerable<User> Users = new List<User>();
        public Register Register { get; set; }
        public IEnumerable<Branch> Branches = new List<Branch>();
    }
}
