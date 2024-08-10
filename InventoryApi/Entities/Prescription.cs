using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Prescription : BaseEntity
    {
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public string DoctorName { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string MedicationDetails { get; set; }
        public string DosageInstructions { get; set; }

    }
}
