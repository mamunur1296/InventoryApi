using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Purchase : BaseModel
    {
        [Required(ErrorMessage = "Purchase Date is required.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        public string SupplierID { get; set; }
        public Supplier Supplier { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }

    }
}
