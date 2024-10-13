using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Shift : BaseModel
    {
        public string ShiftName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
