namespace InventoryApi.Entities
{
    public class SubMenuRole
    {
        public string SubMenuId { get; set; }
        public SubMenu SubMenu { get; set; }
        public string? RoleId { get; set; }
        public ApplicationRole? Role { get; set; }
    }
}
