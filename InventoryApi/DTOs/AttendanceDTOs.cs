using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class AttendanceDTOs : BaseDTOs
    {
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public EmployeeDTOs Employee { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public TimeSpan CheckInTime { get; set; }

        [Required]
        public TimeSpan CheckOutTime { get; set; }

        public bool IsPresent { get; set; }
    }
}
