using InventoryApi.DTOs;

namespace InventoryApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<(bool isSucceed, string userId)> CreateUserAsync(RegistrationDTOs model);
        Task<bool> SigninUserAsync(string userName, string password);
        Task<string> GetUserIdAsync(string userName);
        Task<UserDTO> GetUserDetailsAsync(string userId);
        Task<(string userId, string FirstName, string LastName, string UserName, string email, IList<string> roles)> GetUserDetailsByUserNameAsync(string userName);
        Task<string> GetUserNameAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<List<UserDTO>> GetAllUsersAsync();


        Task<bool> UpdateUserProfile(UserDTO model);

        Task<(bool Success, string ErrorMessage)> ChangePassword(string OldPassword, string newPassword, string Userid);
    }
}
