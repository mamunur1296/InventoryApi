namespace InventoryUi.Services.Interface
{
    public interface IUtilityHelper
    {
        Task<bool> IsDuplicate(IEnumerable<object> data, string key, string val);
    }
}
