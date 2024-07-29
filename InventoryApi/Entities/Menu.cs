using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class Menu : BaseEntity
    {
        public string Name { get; set; }
        public string? Url { get; set; }
        public ICollection<SubMenu>? SubMenus { get; set; }
        public ICollection<MenuRole>? MenuRoles { get; set; }
    }
}
