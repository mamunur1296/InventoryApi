using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class PosViewModel
    {
        public Product Product { get; set; } = new Product();
        public List<Product> Products { get; set;} = new List<Product>();
    }
}
