using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class ActionRole 
    {
        public string ActionId { get; set; }
        public ActionName Action { get; set; }

        // Foreign keys for MenuRole
        public string MenuId { get; set; }
        public string RoleId { get; set; }
        public MenuRole MenuRole { get; set; }

        // Foreign keys for SubMenuRole
        public string SubMenuId { get; set; }
        public SubMenuRole SubMenuRole { get; set; }
    }
}
