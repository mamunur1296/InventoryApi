using InventoryApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryApi.DTOs
{
    public class CategoryDTOs
    {
        
        public string id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, ErrorMessage = "Category name cannot be longer than 255 characters.")]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string? ParentCategoryID { get; set; }

        //public Category ParentCategory { get; set; }

        //public ICollection<Category> SubCategories { get; set; }
        //public ICollection<Product> Products { get; set; }

    }
}
