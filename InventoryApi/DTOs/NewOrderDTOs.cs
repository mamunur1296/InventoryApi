namespace InventoryApi.DTOs
{
    public class NewOrderDTOs :BaseDTOs
    {
        public List<ProductDTOs> ProductsListFromSession { get; set; } = new List<ProductDTOs>();
        public CustomerDTOs? CustomerLIstFromSession { get; set; } = new CustomerDTOs();
        public string EmployeeId { get; set; }
       
    }
}
