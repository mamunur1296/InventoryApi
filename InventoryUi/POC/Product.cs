namespace InventoryUi.POC
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
    public class ProductSuggestionViewModel
    {
        public string SearchTerm { get; set; }
        public List<Product> SuggestedProducts { get; set; } = new List<Product>();
    }


}
