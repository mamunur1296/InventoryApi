using System.Text.Json.Serialization;

namespace InventoryApi.DTOs
{
    public class BaseDTOs
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        
        public DateTime UpdateDate { get; private set; }
        
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public void SetUpdateDate(DateTime updateDate)
        {
            UpdateDate = updateDate;
        }
    }
}
