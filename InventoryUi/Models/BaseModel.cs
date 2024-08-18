namespace InventoryUi.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; private set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        // Optional: A method to update UpdateDate if needed
        public void SetUpdateDate(DateTime updateDate)
        {
            UpdateDate = updateDate;
        }
    }
}
