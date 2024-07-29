using InventoryApi.Entities.Base;

namespace InventoryApi.Entities
{
    public class SubMenu : BaseEntity
    {
        public string Name { get; set; }
        public string? Url { get; set; }
        public string MenuId { get; set; }
        public Menu Menu { get; set; }
        public ICollection<SubMenuRole>? SubMenuRoles { get; set; }
    }
}
