namespace InventoryApi.DTOs
{
    public class MenuDto
    {
        public string MenuId { get; set; }
        public List<SubMenuDto> SubMenuIds { get; set; }
    }
}
