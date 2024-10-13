using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Department : BaseModel
    {
        public string DepartmentName { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
