namespace InventoryApi.DTOs
{
    public class RoleMenuMappingDto
    {
        public string RoleId { get; set; }
        public List<string> MenuIds { get; set; }
        public List<string> SubMenuIds { get; set; }
    }
}
