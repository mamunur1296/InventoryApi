using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string FirstName, string LastName, string phoneNumber, List<string> roles)
        {
            var user = new ApplicationUser()
            {
                FirstName = FirstName,
                LastName = LastName,
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }

            var addUserRole = await _userManager.AddToRolesAsync(user, roles);
            if (!addUserRole.Succeeded)
            {
                throw new ValidationException(addUserRole.Errors);
            }
            return (result.Succeeded, user.Id);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }

            if (user.UserName == "Administrator" || user.UserName == "admin")
            {
                throw new Exception("You can not delete system or admin user");
                //throw new BadRequestException("You can not delete system or admin user");
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<List<(string id, string FirstName, string LastName, string Phone, string userName, string email)>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.PhoneNumber,
                x.UserName,
                x.Email,

            }).ToListAsync();

            return users.Select(user => (user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.UserName, user.Email)).ToList();
        }



        public async Task<(string userId, string FirstName, string LastName, string UserName, string email, IList<string> roles)> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.FirstName, user.LastName, user.UserName, user.Email, roles);
        }



        public async Task<(string userId, string FirstName, string LastName, string UserName, string email, IList<string> roles)> GetUserDetailsByUserNameAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.FirstName, user.LastName, user.UserName, user.Email, roles);
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return await _userManager.GetUserNameAsync(user);
        }

        public async Task<bool> IsUniqueUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName) == null;
        }

        public async Task<bool> SigninUserAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserProfile(string id, string FirstName, string LastName, string email, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = email;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
