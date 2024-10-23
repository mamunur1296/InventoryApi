using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class PayrollDTOs : BaseDTOs
    {
        public string EmployeeId { get; set; }

       
        //public EmployeeDTOs Employee { get; set; }
        [Precision(18, 2)]
        public decimal BaseSalary { get; set; }

        [Precision(18, 2)]
        public decimal? Bonus { get; set; }

        [Precision(18, 2)]
        public decimal?Deductions { get; set; }


        [Precision(18, 2)]
        public decimal NetSalary { get; set; }
        public DateTime ? PaymentDate { get; set; } 
    }
}
