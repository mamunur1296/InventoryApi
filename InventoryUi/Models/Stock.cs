﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Stock : BaseModel
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public string ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Warehouse ID is required.")]
        public string WarehouseID { get; set; }

        [ForeignKey("WarehouseID")]
        public Warehouse Warehouse { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
        public int Quantity { get; set; }
    }
}
