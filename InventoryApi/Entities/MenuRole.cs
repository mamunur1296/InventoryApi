namespace InventoryApi.Entities
{
    public class MenuRole
    {
        public string MenuId { get; set; }
        public Menu Menu { get; set; }
        public string? RoleId { get; set; }
        public ApplicationRole? Role { get; set; }
    }
}
