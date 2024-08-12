namespace InventoryUi.Models
{
    public class MenuRole
    {
        public string MenuId { get; set; }
        public Menu Menu { get; set; }
        public string? RoleId { get; set; }
        public Roles? Role { get; set; }
    }
}
