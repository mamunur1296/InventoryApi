using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DTOs
{
    public class ProductDTOs : BaseDTOs
    {


        public string ProductName { get; set; }
        public string Description { get; set; }
        public string CategoryID { get; set; }

        
        //public Category Category { get; set; }
        public string SupplierID { get; set; }

       
        //public Supplier Supplier { get; set; }
        public string QuantityPerUnit { get; set; }
        public string UnitMasterId { get; set; }

        //[ForeignKey("UnitMasterId")]
        //public UnitMaster UnitMaster { get; set; }
        public string UnitChildId { get; set; }

        //[ForeignKey("UnitChildId")]
        //public UnitChild UnitChild { get; set; }
        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
        public string BatchNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public string ImageURL { get; set; }
        [Precision(18, 2)]
        public decimal Weight { get; set; }
        public string Dimensions { get; set; }

    }
}
