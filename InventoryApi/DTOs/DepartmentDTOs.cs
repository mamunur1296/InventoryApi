using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class DepartmentDTOs : BaseDTOs
    {
        public string ?DepartmentName { get; set; }

        [MaxLength(200)]
        public string ?Description { get; set; }

        //public ICollection<EmployeeDTOs> Employees { get; set; }
    }
}
