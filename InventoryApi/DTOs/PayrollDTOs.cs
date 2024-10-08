using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class PayrollDTOs : BaseDTOs
    {
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeDTOs Employee { get; set; }

        [Required(ErrorMessage = "Base salary is required")]
        [Precision(18, 2)]
        public decimal BaseSalary { get; set; }

        [Precision(18, 2)]
        public decimal Bonus { get; set; }

        [Precision(18, 2)]
        public decimal Deductions { get; set; }

        [Required(ErrorMessage = "Net salary is required")]
        [Precision(18, 2)]
        public decimal NetSalary { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        public DateTime PaymentDate { get; set; }
    }
}
