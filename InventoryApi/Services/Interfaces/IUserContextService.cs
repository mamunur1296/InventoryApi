namespace InventoryApi.Services.Interfaces
{
    public interface IUserContextService
    {
        string UserName { get; }
        string UserId { get; }
        string UserRole { get; }
    }
}
