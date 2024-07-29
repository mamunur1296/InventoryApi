using InventoryApi.Entities;

namespace InventoryApi.DTOs
{
    public class MenuDTOs
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string? Url { get; set; }
        //public ICollection<SubMenu>? SubMenus { get; set; }
        //public ICollection<MenuRole>? MenuRoles { get; set; }
    }
}
