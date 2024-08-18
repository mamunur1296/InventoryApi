using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class PrescriptionDTOs : BaseDTOs
    {
      
        public string CustomerID { get; set; }
       // public Customer Customer { get; set; }
        public string DoctorName { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string MedicationDetails { get; set; }
        public string DosageInstructions { get; set; }

    }
}
