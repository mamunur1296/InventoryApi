namespace InventoryApi.DTOs
{
    public class NewOrderDTOs
    {
        public List<ProductDTOs> ProductsListFromSession { get; set;} = new List<ProductDTOs>();
        public CustomerDTOs ? CustomerLIstFromSession {  get; set; } = new CustomerDTOs();
    }
}
