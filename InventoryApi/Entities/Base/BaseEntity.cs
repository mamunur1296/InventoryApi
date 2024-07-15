using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; private set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public BaseEntity()
        {
            UpdateDate = DateTime.Now;
        }
    }
}
