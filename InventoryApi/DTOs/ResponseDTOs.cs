using System.Net;

namespace InventoryApi.DTOs
{
    public class ResponseDTOs<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode Status { get; set; }
        public ResponseDTOs()
        {
            Success = true;
        }
    }
}
