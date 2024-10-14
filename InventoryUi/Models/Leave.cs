
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Leave : BaseModel
    {
        [DisplayName("Employee")]
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Leave type is required")]
        [DisplayName("Leave Type")]
        public string LeaveType { get; set; } // e.g. Sick, Casual, Vacation

        [Required(ErrorMessage = "Start date is required")]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [DisplayName("Reason")]
        public string Reason { get; set; }
        [DisplayName("IsApproved")]
        public bool IsApproved { get; set; }
    }

    public enum LeaveType
    {
        Sick, Casual, EarnLeave, Other
    }
}
