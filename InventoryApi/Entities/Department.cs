using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; }

        [MaxLength(200)]
        public string ?Description { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
