using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Leave : BaseEntity
    {
        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Leave type is required")]
        public string LeaveType { get; set; } // e.g. Sick, Casual, Vacation

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        public string ?Reason { get; set; }

        public bool IsApproved { get; set; }
    }

    public enum LeaveType
    {
        Sick, Casual, EarnLeave, Other
    }
}
