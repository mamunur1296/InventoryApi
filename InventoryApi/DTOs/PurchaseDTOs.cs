using InventoryApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class PurchaseDTOs : BaseDTOs
    {
        [Required(ErrorMessage = "Purchase Date is required.")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        public string SupplierID { get; set; }

        [ForeignKey("SupplierID")]
        //public Supplier Supplier { get; set; }

        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }
        //public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
