using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Shift : BaseEntity
    {
        public string ShiftName { get; set; }
        public TimeSpan ?StartTime { get; set; }
        public TimeSpan ?EndTime { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
