
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Holiday : BaseModel
    {
        [Required]
        [DisplayName("Name")]
        public string HolidayName { get; set; }

        [Required]
        [DisplayName("Date")]
        public DateTime Date { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
