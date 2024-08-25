using InventoryApi.DTOs;
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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHelperServicess _helperServicess;
        



        public UserService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IHelperServicess helperServicess
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _helperServicess = helperServicess;
        }

        public async Task<(bool Success, string ErrorMessage)> ChangePassword(string OldPassword, string newPassword, string Userid)
        {
            var user = await _userManager.FindByIdAsync(Userid);
            if (user == null)
            {
                // Handle user not found
                throw new NotFoundException("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, OldPassword, newPassword);
            if (result.Succeeded)
            {
                // Password change successful
                return (true, null);
            }

            // Collect error messages
            var errors = result.Errors.Select(e => e.Description).ToList();
            var errorMessage = string.Join("; ", errors);

            return (false, errorMessage);
        }

        public async Task<(bool isSucceed, string userId)> CreateUserAsync(RegistrationDTOs model)
        {
            
            var result = await _helperServicess.CreateUserAndCustomerAsync(model);

            return (result.isSucceed, result.userId);
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

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .Select(x => new UserDTO
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    Email = x.Email,
                    UserImg = x.UserImg,
                    isApproved = x.isApproved,
                    isApprovedByAdmin = x.isApprovedByAdmin,
                    isEmployee = x.isEmployee
                })
                .ToListAsync();

            return users;
        }





        public async Task<UserDTO> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Email = user.Email,
                UserImg = user.UserImg,
                isApproved = user.isApproved,
                isApprovedByAdmin = user.isApprovedByAdmin,
                isEmployee = user.isEmployee,
                Roles = roles.ToList()
            };
            return userDto;
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
            user.FirstName = !string.IsNullOrEmpty(firstName) ? firstName.Trim() : user.FirstName; // Update only if firstName has a value
            user.LastName = !string.IsNullOrEmpty(lastName) ? lastName.Trim() : user.LastName; // Update only if lastName has a value
            user.Email = !string.IsNullOrEmpty(email) ? email : user.Email;
            user.PhoneNumber = !string.IsNullOrEmpty(PhoneNumber) ? PhoneNumber : user.PhoneNumber;
            user.UserImg = !string.IsNullOrEmpty(img) ? img : user.UserImg;
            user.Job = !string.IsNullOrEmpty(Job) ? Job : user.Job;
            user.Country = !string.IsNullOrEmpty(Country) ? Country : user.Country;
            user.Address = !string.IsNullOrEmpty(Address) ? Address : user.Address;
            user.NID = !string.IsNullOrEmpty(NID) ? NID : user.NID;
            user.About = !string.IsNullOrEmpty(about) ? about.Trim() : user.About;
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
