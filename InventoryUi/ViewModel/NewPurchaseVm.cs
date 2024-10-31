using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class NewPurchaseVm
    {
        public Company Company { get; set; } = new Company();
        public Branch Branch { get; set; } = new Branch();
    }
}
