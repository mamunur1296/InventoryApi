using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class OrderDTOs : BaseDTOs
    {
        
        public string CustomerID { get; set; }
        //public Customer Customer { get; set; }
        public string? EmployeeID { get; set; }
        //public Employee Employee { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        //public Shipper Shipper { get; set; }
        public decimal Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public string? PrescriptionID { get; set; }
        //public Prescription Prescription { get; set; }
        public string? PaymentStatus { get; set; }
        public string? OrderStatus { get; set; }
        //public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
