using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class ProductListVm
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
