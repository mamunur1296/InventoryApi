﻿using InventoryApi.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Entities
{
    public class PurchaseDetail : BaseEntity
    {
        [Required(ErrorMessage = "Purchase ID is required.")]
        public string PurchaseID { get; set; }

        [ForeignKey("PurchaseID")]
        public Purchase Purchase { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public string ProductID { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        public int Quantity { get; set; }

        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }

        [Precision(18, 2)]
        public decimal Discount { get; set; }

        [Precision(18, 2)]
        public decimal TotalPrice => Quantity * UnitPrice - Discount;
    }
}