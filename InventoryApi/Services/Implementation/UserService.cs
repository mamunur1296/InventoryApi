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
            if (!result.isSucceed)
            {
                throw new ValidationException(result.errorMessage);
            }
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
                    isEmployee = x.isEmployee,
                    NID=x.NID,
                    About=x.About,
                    Address=x.Address,
                    Country=x.Country,
                    Job=x.Job,
                    CompanyId=x.CompanyId,
                    BranchId=x.BranchId,
                   
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
                Roles = roles.ToList(),
                Job = user.Job,
                Country = user.Country,
                Address = user.Address,
                About = user.About,
                NID = user.NID,
                BranchId = user.BranchId,
                CompanyId = user.CompanyId,
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
            var existingUser = await _userManager.FindByNameAsync(userName);

            if (existingUser == null)
            {
                throw new ValidationException("User does not exist. Please register before trying to sign in.");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: true, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new ValidationException("Invalid username or password. Please try again.");
            }

            return true;
        }



        public async Task<bool> UpdateUserProfile(UserDTO model)
        {
            // Validate input data
            if (string.IsNullOrEmpty(model.Id))
            {
                throw new ValidationException("User ID must be provided.");
            }

            if (model.Roles == null || model.Roles.Count == 0)
            {
                throw new ValidationException("Role names must be provided.");
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                throw new ValidationException("User not found.");
            }

            // Check if roles exist
            var roleExists = true;
            foreach (var role in model.Roles)
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
            user.FirstName = !string.IsNullOrEmpty(model.FirstName) ? model.FirstName.Trim() : user.FirstName; // Update only if firstName has a value
            user.LastName = !string.IsNullOrEmpty(model.LastName) ? model.LastName.Trim() : user.LastName; // Update only if lastName has a value
            user.Email = !string.IsNullOrEmpty(model.Email) ? model.Email : user.Email;
            user.PhoneNumber = !string.IsNullOrEmpty(model.PhoneNumber) ? model.PhoneNumber : user.PhoneNumber;
            user.UserImg = !string.IsNullOrEmpty(model.UserImg) ? model.UserImg : user.UserImg;
            user.Job = !string.IsNullOrEmpty(model.Job) ? model.Job : user.Job;
            user.Country = !string.IsNullOrEmpty(model.Country) ? model.    Country : user.Country;
            user.Address = !string.IsNullOrEmpty(model.Address) ? model.Address : user.Address;
            user.NID = !string.IsNullOrEmpty(model.NID) ? model.NID : user.NID;
            user.About = !string.IsNullOrEmpty(model.About) ? model.About.Trim() : user.About;
            user.isApproved=model.isApproved;
            user.isApprovedByAdmin=model.isApprovedByAdmin;
            user.isEmployee=model.isEmployee;
            user.BranchId=model.BranchId;
            user.CompanyId = model.CompanyId;
            // Perform update operation
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors);
            }

            // Update user roles
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = userRoles.Except(model.Roles).ToList();
            var rolesToAdd = model.Roles.Except(userRoles).ToList();

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
