using InventoryUi.DTOs;
using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class NewOrderVm : BaseModel
    {
        public string SearchTerm { get; set; }
        public IEnumerable<Product> Products { get; set; } = new List<Product>(); // Default empty list of Products
        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
        public List<Product> ProductsListFromSession { get; set;} = new List<Product>();
        public Customer ? CustomerLIstFromSession {  get; set; } = new Customer();
        public Product Product { get; set; }= new Product();
        public Customer Customer { get; set; }= new Customer();
        public Category Category { get; set; }= new Category();
        public Supplier Supplier { get; set; }= new Supplier();
        public UnitMaster UnitMaster { get; set; }= new UnitMaster();
        public UnitChild UnitChild { get; set; }= new UnitChild();
        public IEnumerable<Category> CategoryLIst { get; set; } = new List<Category>();
        public IEnumerable<Supplier> Suppliers { get; set; } = new List<Supplier>();
        public IEnumerable<UnitMaster> unitMasters { get; set; }= new List<UnitMaster>();
        public IEnumerable<UnitChild> unitChildrens { get; set; } = new List<UnitChild>();
        public bool IsPaymentButtonEnabled { get; set; }=false;
        public string ? EmployeeId { get; set; }
        public string ? newOrderId { get; set; }
        
    }

}
