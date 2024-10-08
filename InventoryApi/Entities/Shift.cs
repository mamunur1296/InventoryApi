using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Shift : BaseEntity
    {
        public string ShiftName { get; set; }

        [Required(ErrorMessage = "Shift start time is required")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Shift end time is required")]
        public TimeSpan EndTime { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
