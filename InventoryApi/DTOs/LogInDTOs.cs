using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class LogInDTOs
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
