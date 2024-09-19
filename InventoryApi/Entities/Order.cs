using InventoryApi.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Entities
{
    public class Order : BaseEntity
    {
        public string ? CustomerID { get; set; }
        public Customer? Customer { get; set; }
        public string? EmployeeID { get; set; }
        public Employee? Employee { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public Shipper? Shipper { get; set; }
        [Precision(18, 2)]
        public decimal? Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public string? PrescriptionID { get; set; }
        public Prescription? Prescription { get; set; }
        public string? PaymentStatus { get; set; }
        public string? OrderStatus { get; set; }
        public string InvoiceNumber { get; set; }
        public bool ? IsHold { get; set; }
        public string ? HoldReason { get; set; }
        [Precision(18, 2)]
        public decimal? TotalAmountBeforeDiscount { get; set; }
        [Precision(18, 2)]
        public decimal? TotalAmountAfterDiscount { get; set; }
        [Precision(18, 2)]
        public decimal? TotalAmount { get; set; }
        [Precision(18, 2)]
        public decimal? Tax { get; set; }
        [Precision(18, 2)]
        public decimal? Vat { get; set; }
        [Precision(18, 2)]
        public decimal? DelivaryCharge { get; set; }
        [Precision(18, 2)]
        public decimal? OtherExpances { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}
