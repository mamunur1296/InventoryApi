using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Prescription : BaseModel
    {
        [DisplayName("Customer")]
        public string CustomerID { get; set; }
        [DisplayName("Customer")]
        public Customer Customer { get; set; }
        [DisplayName("Doctor Name")]
        public string DoctorName { get; set; }
        [DisplayName("Date")]
        public DateTime PrescriptionDate { get; set; }
        [DisplayName("Medication Details")]
        public string MedicationDetails { get; set; }
        [DisplayName("Dosage Instructions")]
        public string DosageInstructions { get; set; }
    }
}
