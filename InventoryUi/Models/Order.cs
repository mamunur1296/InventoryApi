using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Order : BaseModel
    {
        [DisplayName("Customer")]
        public string CustomerID { get; set; }
        [DisplayName("Name")]
        public Customer Customer { get; set; }
        [DisplayName("Employee")]
        public string? EmployeeID { get; set; }
        [DisplayName("Name")]
        public Employee Employee { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Required Date")]
        public DateTime? RequiredDate { get; set; }
        [DisplayName("Shipped Date")]
        public DateTime? ShippedDate { get; set; }
        [DisplayName("Ship Via")]
        public int? ShipVia { get; set; }
        [DisplayName("Shipper")]
        public Shipper Shipper { get; set; }
        [DisplayName("Freight")]
        public decimal Freight { get; set; }
        [DisplayName("Ship Name")]
        public string ShipName { get; set; }
        [DisplayName("Ship Address")]
        public string ShipAddress { get; set; }
        [DisplayName("Ship City")]
        public string ShipCity { get; set; }
        [DisplayName("Ship Region")]
        public string ShipRegion { get; set; }
        [DisplayName("Ship Postal Code")]
        public string ShipPostalCode { get; set; }
        [DisplayName("Ship Country")]
        public string ShipCountry { get; set; }
        [DisplayName("Prescription")]
        public string? PrescriptionID { get; set; }
        [DisplayName("Prescription")]
        public Prescription Prescription { get; set; }
        [DisplayName("Payment Status")]
        public string PaymentStatus { get; set; }
        [DisplayName("Order Status")]
        public string OrderStatus { get; set; }
        [DisplayName("Order Details")]
        public string InvoiceNumber { get; set; }
        public bool? IsHold { get; set; }
        public string? HoldReason { get; set; }
        public decimal? TotalAmountBeforeDiscount { get; set; }
        public decimal? TotalAmountAfterDiscount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Vat { get; set; }
        public decimal? DelivaryCharge { get; set; }
        public decimal? OtherExpances { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
