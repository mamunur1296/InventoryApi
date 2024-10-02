namespace InventoryApi.DTOs
{
    public class PurchaseItemDTOs
    {
        public string SupplierID { get; set; }
        public List<PurchaseProductItemDTOs> Products { get; set; }
    }
    public class PurchaseProductItemDTOs
    {
        public string ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}
