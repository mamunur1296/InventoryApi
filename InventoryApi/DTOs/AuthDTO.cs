using System.Net;

namespace InventoryApi.DTOs
{
    public class AuthDTO
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
