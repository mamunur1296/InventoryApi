using System.ComponentModel;

namespace InventoryUi.Models
{
    public class Review : BaseModel
    {
        [DisplayName("Product")]
        public string ProductID { get; set; }
        [DisplayName("Product")]
        public Product Product { get; set; }
        [DisplayName("Customer")]
        public string CustomerID { get; set; }
        [DisplayName("Customer")]
        public Customer Customer { get; set; }
        [DisplayName("Rating")]
        public int Rating { get; set; }
        [DisplayName("Command")]
        public string ReviewText { get; set; }
        [DisplayName("ReviewDate")]
        public DateTime ReviewDate { get; set; }
    }
}
