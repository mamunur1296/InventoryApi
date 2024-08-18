using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApi.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; private set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public void SetUpdateDate(DateTime updateDate)
        {
            UpdateDate = updateDate;
        }
    }
}
