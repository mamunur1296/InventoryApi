using InventoryApi.DTOs;

namespace InventoryApi.Services.Interfaces
{
    public interface IHelperServicess
    {
        Task<(bool isSucceed, string userId, string errorMessage)> CreateUserAndCustomerAsync(RegistrationDTOs model);
        Task<(bool isSucceed, string CustomerId, string errorMessage)> CreateCustomerByAdmin(string id);
        Task<(bool isSucceed, string CustomerId, string errorMessage)> CreateEmployeeByAdmin(string id);
        
    }
}
