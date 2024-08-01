namespace InventoryUi.Models
{
    public class Warehouse : BaseModel
    {
        public string? Location { get; set; }
        public string? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public Company? Company { get; set; }
        //public ICollection<Product>? Products { get; set; }

    }
}
