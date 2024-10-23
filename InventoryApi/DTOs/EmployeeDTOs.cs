using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApi.DTOs
{
    public class EmployeeDTOs : BaseDTOs
    {
        
        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }
        public string? Title { get; set; }
        public string? TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? HomePhone { get; set; }
        public string? Extension { get; set; }
        public byte[]? Photo { get; set; }
        public string? Notes { get; set; }
        public string? ManagerId { get; set; }
        public int? ReportsTo { get; set; }
        public string? PhotoPath { get; set; }
        public bool? IsEmp { get; set; }
        public string? UserId { get; set; }
        public string? CompanyId { get; set; }
        public string? BranchId { get; set; }
        [Precision(18, 2)]
        public decimal? Salary { get; set; } = null;

        public string UserName { get; set; }

        public string Password { get; set; }
  
        public string Email { get; set; }
  
        public string DepartmentId { get; set; }
        //public Department Department { get; set; }

        //public ICollection<Attendance> Attendances { get; set; }
        //public ICollection<Payroll> Payrolls { get; set; }
        // public ApplicationUser? User { get; set; }
        // public Employee Manager { get; set; }
        // public ICollection<Employee> Subordinates { get; set; }
        // public ICollection<Order> Orders { get; set; }

    }
}
