namespace InventoryUi.Models
{
    public class Menu : BaseModel
    {
        public string Name { get; set; }
        public string? Url { get; set; }
        public ICollection<SubMenu>? SubMenus { get; set; }
        public ICollection<MenuRole>? MenuRoles { get; set; }
    }
}
