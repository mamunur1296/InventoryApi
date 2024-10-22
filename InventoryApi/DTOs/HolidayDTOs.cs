using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class HolidayDTOs : BaseDTOs
    {
        [Required]
        public string HolidayName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string ?Description { get; set; }
    }
}
