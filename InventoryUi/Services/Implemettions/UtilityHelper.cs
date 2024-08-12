using InventoryUi.Services.Interface;

namespace InventoryUi.Services.Implemettions
{
    public class UtilityHelper : IUtilityHelper
    {
        public async Task<bool> IsDuplicate(IEnumerable<object> data, string key, string val)
        {
            return await Task.Run(() =>
            {
                return data.Any(item =>
                {
                    var propertyInfo = item.GetType().GetProperty(key);
                    if (propertyInfo == null) return false;
                    var propertyValue = propertyInfo.GetValue(item, null)?.ToString();
                    return propertyValue?.Trim().Equals(val.Trim(), StringComparison.OrdinalIgnoreCase) ?? false;
                });
            });
        }  
    }
}
