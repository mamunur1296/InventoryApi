using InventoryApi.Entities.Base;


namespace InventoryApi.Entities
{
    public class Category : BaseEntity
    {

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string? ParentCategoryID { get; set; }

        //[ForeignKey("ParentCategoryID")]
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
