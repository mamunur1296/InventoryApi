using InventoryUi.Models;

namespace InventoryUi.POC
{
    public class MenuViewModel
    {
        public IEnumerable<Menu> Menus { get; set; } = new List<Menu>();
        public Menu NewMenu { get; set; } = new Menu();
    }
}
