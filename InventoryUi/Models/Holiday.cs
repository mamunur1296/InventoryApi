
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Holiday : BaseModel
    {
        [Required]
        public string HolidayName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}
