using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryUi.Models
{
    public class Category : BaseModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, ErrorMessage = "Category name cannot be longer than 255 characters.")]
        [DisplayName("Category Name ")]
        public string CategoryName { get; set; }
        [DisplayName("Description ")]
        public string Description { get; set; }
        [DisplayName("Sub Catagory ")]
        public string? ParentCategoryID { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
