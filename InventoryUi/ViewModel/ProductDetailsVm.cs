﻿using InventoryUi.Models;

namespace InventoryUi.ViewModel
{
    public class ProductDetailsVm
    {
        public Product Product { get; set; }
        public IEnumerable<Product> reletedProduct { get; set; }= new List<Product>();
    }
}