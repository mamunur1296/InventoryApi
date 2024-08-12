namespace InventoryUi.Models
{
    public class SubMenu : BaseModel
    {
        public string Name { get; set; }
        public string? Url { get; set; }
        public string MenuId { get; set; }
        public Menu Menu { get; set; }
        public ICollection<SubMenuRole>? SubMenuRoles { get; set; }
    }
}
