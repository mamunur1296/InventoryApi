using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class Holiday : BaseEntity
    {
        [Required]
        public string HolidayName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string ?Description { get; set; }
    }
}
