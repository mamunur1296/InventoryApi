namespace InventoryApi.DTOs
{
    public class SalesSummaryDTOs
    {
        public decimal TodaySalesAmount { get; set; }
        public int TodaySalesUnit { get; set; }
        public decimal LastDaySalesAmount { get; set; }
        public int LastDaySalesUnit { get; set; }
        public decimal CurrentMonthSalesAmount { get; set; }
        public int CurrentMonthSalesUnit { get; set; }
        public decimal LastMonthSalesAmount { get; set; }
        public int LastMonthSalesUnit { get; set; }
    }
}
