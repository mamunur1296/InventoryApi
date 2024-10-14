using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Shift : BaseModel
    {
        [DisplayName("Shift")]
        public string ShiftName { get; set; }
        [DisplayName("Start Time")]
        public TimeSpan? StartTime { get; set; }
        [DisplayName("End Time")]
        public TimeSpan? EndTime { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
