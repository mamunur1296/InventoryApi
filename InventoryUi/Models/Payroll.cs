using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Payroll : BaseModel
    {
        [DisplayName("Employee")]
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Base salary is required")]
        [DisplayName("Salary")]
        public decimal BaseSalary { get; set; }
        [DisplayName("Bonus")]
        public decimal Bonus { get; set; }

        [DisplayName("Deductions")]
        public decimal ?Deductions { get; set; }

        [Required(ErrorMessage = "Net salary is required")]
        [DisplayName("Net Salary")]
        public decimal NetSalary { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        [DisplayName("Payment Date")]
        public DateTime PaymentDate { get; set; }
    }
}
