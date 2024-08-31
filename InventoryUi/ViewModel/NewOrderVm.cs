using InventoryUi.DTOs;
using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class NewOrderVm
    {
        public string SearchTerm { get; set; }
        public IEnumerable<Product> Products { get; set; } = new List<Product>(); // Default empty list of Products
        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
        public List<Product> ProductsListFromSession { get; set;} = new List<Product>();
        public Customer CustomerLIstFromSession {  get; set; } = new Customer();
        public bool IsPaymentButtonEnabled { get; set; }=false;
    }

}
