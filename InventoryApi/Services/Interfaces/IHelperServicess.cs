using InventoryApi.DTOs;

namespace InventoryApi.Services.Interfaces
{
    public interface IHelperServicess
    {
        Task<(bool isSucceed, string userId, string errorMessage)> CreateUserAndCustomerAsync(RegistrationDTOs model);
    }
}
