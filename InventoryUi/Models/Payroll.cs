using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Payroll : BaseModel
    {
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Base salary is required")]

        public decimal BaseSalary { get; set; }

        public decimal Bonus { get; set; }

 
        public decimal Deductions { get; set; }

        [Required(ErrorMessage = "Net salary is required")]

        public decimal NetSalary { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        public DateTime PaymentDate { get; set; }
    }
}
