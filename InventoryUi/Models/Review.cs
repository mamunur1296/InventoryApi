﻿namespace InventoryUi.Models
{
    public class Review : BaseModel
    {
        public string ProductID { get; set; }
        public Product Product { get; set; }
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
