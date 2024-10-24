using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Employee : BaseModel
    {
        [Required]

        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Title")]
        public string ?Title { get; set; }
        [DisplayName("Title Of Courtesy")]
        public string? TitleOfCourtesy { get; set; }
        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }
        [DisplayName("Hire Date")]
        public DateTime ?HireDate { get; set; } 
        [DisplayName("Address")]
        public string? Address { get; set; }
        [DisplayName("City")]
        public string? City { get; set; }
        [DisplayName("Region")]
        public string? Region { get; set; }
        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }
        [DisplayName("Country")]
        public string? Country { get; set; }
        [DisplayName("Phone")]
        public string? HomePhone { get; set; }
        [DisplayName("Extension")]
        public string? Extension { get; set; }
        
        [DisplayName("Notes")]
        public string? Notes { get; set; }
        [DisplayName("Reports")]
        public int? ReportsTo { get; set; }
        [DisplayName("Photo")]
        public byte[]? Photo { get; set; }
        [DisplayName("Photo")]
        public string? PhotoPath { get; set; }
        [DisplayName("Photo")]
         public List<IFormFile>? Files { get; set; } 
        [DisplayName("User")]
        public string ? UserId { get; set; }
        public User? User { get; set; }
        [DisplayName("Manager")]
        public string? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public bool ? IsEmp {  get; set; }
        [DisplayName("Company")]
        public string? CompanyId { get; set; }
        [DisplayName("Branch")]
        public string? BranchId { get; set; }
        public ICollection<Employee>? Subordinates { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public decimal? Salary { get; set; } = null;
        public string? DepartmentId { get; set; }
        public Department? Department { get; set; }
        [DisplayName("User Name")]
        public string ?UserName { get; set; }
        [DisplayName("Password")]
        public string ?Password { get; set; }
        [DisplayName("Email")]
        public string ?Email { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
        public ICollection<Payroll>? Payrolls { get; set; }
    }
}
