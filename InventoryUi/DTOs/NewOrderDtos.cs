namespace InventoryUi.DTOs
{
    public class NewOrderDtos
    {
        public string productId { get; set; }
       
 
        public int  SubTotal {  get; set; }
        public int Expances { get; set; }
        public int GrossTotal { get; set; }
        public int PaymentTotal { get; set; }
        public int AmountDue { get; set; }
        public int TotalPaymentAmount { get; set; }
    }
}
