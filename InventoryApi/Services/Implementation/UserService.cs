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
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string firstName, string lastName, string phoneNumber, List<string> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                throw new ValidationException("Role names must be provided.");
            }

            var user = new ApplicationUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
            };

            var roleExists = true;
            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    roleExists = false;
                    break;
                }
            }

            if (!roleExists)
            {
                throw new ValidationException("One or more roles are invalid.");
            }

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

            return (true, user.Id);
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



        public async Task<(string userId, string FirstName, string LastName, string UserName, string email, string img, string PhoneNumber, string NID, string Address, string Job, string Country, string About, IList<string> roles)> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.FirstName, user.LastName, user.UserName, user.Email, user.UserImg, user.PhoneNumber , user.NID, user.Address, user.Job, user.Country,user.About, roles);
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
            if (!result.Succeeded)
            {
                throw new ValidationException("Username or password does not match.");
            }
            return true;
        }


        public async Task<bool> UpdateUserProfile(string id, string firstName, string lastName, string email, string img, string PhoneNumber, string NID, string Address, string Job, string Country, string about, IList<string> roles)
        {
            // Validate input data
            if (string.IsNullOrEmpty(id))
            {
                throw new ValidationException("User ID must be provided.");
            }

            if (roles == null || roles.Count == 0)
            {
                throw new ValidationException("Role names must be provided.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ValidationException("User not found.");
            }

            // Check if roles exist
            var roleExists = true;
            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                {
                    roleExists = false;
                    break;
                }
            }

            if (!roleExists)
            {
                throw new ValidationException("One or more roles are invalid.");
            }

            // Update user properties
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.PhoneNumber=PhoneNumber;
            user.UserImg = img;
            user.Job = Job;
            user.Country = Country;
            user.Address = Address;
            user.NID=NID;
            user.About=about;

            // Perform update operation
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }

            // Update user roles
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = userRoles.Except(roles).ToList();
            var rolesToAdd = roles.Except(userRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    throw new ValidationException(removeResult.Errors);
                }
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    throw new ValidationException(addResult.Errors);
                }
            }

            return true;
        }

       
    }
}
