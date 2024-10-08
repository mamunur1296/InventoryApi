using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class ShiftDTOs : BaseDTOs
    {
        public string ShiftName { get; set; }

        [Required(ErrorMessage = "Shift start time is required")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Shift end time is required")]
        public TimeSpan EndTime { get; set; }

        public ICollection<EmployeeDTOs> Employees { get; set; }
    }
}
