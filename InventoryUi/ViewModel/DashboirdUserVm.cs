using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class DashboirdUserVm
    {
        public IEnumerable<Company> Companies=new List<Company>();
        public User User { get; set; } = new User();
        public IEnumerable<User> Users = new List<User>();
        public Register Register { get; set; } = new Register();
        public IEnumerable<Branch> Branches = new List<Branch>();
        public Company Company { get; set; } = new Company();
        public Branch Branch { get; set; } = new Branch();
    }
}
