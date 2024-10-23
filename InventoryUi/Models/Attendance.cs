
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Attendance : BaseModel
    {
        [DisplayName("Employee")]
        public string EmployeeId { get; set; }


        public Employee Employee { get; set; }
        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Check In Time")]
        public TimeSpan CheckInTime { get; set; }

        [Required]
        [DisplayName("Check Out Time")]
        public TimeSpan CheckOutTime { get; set; }
        [DisplayName("IsPresent")]
        public bool IsPresent { get; set; } = true;
    }
}
