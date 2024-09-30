using InventoryApi.DTOs;

namespace InventoryApi.Services.Interfaces
{
    public interface IDeliveryAddressServices
    {
        Task<DeliveryAddressDTOs> GetByUserIdAsync(string userId);
    }
}
