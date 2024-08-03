using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class ActionRole 
    {
        public string ActionId { get; set; }
        public ActionName Action { get; set; }
        public string SubMenuId { get; set; }  // Add this property
        public SubMenu SubMenu { get; set; }   // Optional navigation property
        public string? RoleId { get; set; }
        public ApplicationRole? Role { get; set; }

    }
}
