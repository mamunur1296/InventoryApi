namespace InventoryApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string firstName, string lastName, string phoneNumber, List<string> roles);
        Task<bool> SigninUserAsync(string userName, string password);
        Task<string> GetUserIdAsync(string userName);
        Task<(string userId, string FirstName, string LastName, string UserName, string email,string img, string PhoneNumber, string NID, string Address, string Job , string Country,string About, IList<string> roles)> GetUserDetailsAsync(string userId);
        Task<(string userId, string FirstName, string LastName, string UserName, string email, IList<string> roles)> GetUserDetailsByUserNameAsync(string userName);
        Task<string> GetUserNameAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<List<(string id, string FirstName, string LastName, string Phone, string userName, string email)>> GetAllUsersAsync();

        Task<bool> UpdateUserProfile(string id, string FirstName, string LastName, string email, string img, string PhoneNumber, string NID, string Address, string Job, string Country,string about, IList<string> roles);
    }
}
