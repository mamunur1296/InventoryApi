using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class ErrorLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ? Message { get; set; }
        public string ?StackTrace { get; set; }
        public string ?Source { get; set; }
        public string ?UserId { get; set; }
        public string ?UserIpAddress { get; set; }
        public DateTime LogDate { get; set; }
        public string ?Url { get; set; }
        public string ?ExceptionType { get; set; }
        public string ?InnerException { get; set; }
        public string ?HttpMethod { get; set; }
        public string ?RequestHeaders { get; set; }
        public string ?FormData { get; set; }
    }
}
