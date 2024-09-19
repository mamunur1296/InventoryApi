using InventoryApi.DataContext;
using InventoryApi.DTOs;
using InventoryApi.Exceptions;
using InventoryApi.Entities;
using InventoryApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InventoryApi.Services.Implementation
{
    public class HelperServicess : IHelperServicess
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IBaseServices<CustomerDTOs> _customer;
        private readonly ApplicationDbContext _context;

        public HelperServicess(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, 
            IBaseServices<CustomerDTOs> customer, 
            ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _customer = customer;
            _context = context;
        }

        public async Task<(bool isSucceed, string userId, string errorMessage)> CreateUserAndCustomerAsync(RegistrationDTOs model)
        {

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Ensure roles are provided
                    if (model.Roles == null || model.Roles.Count == 0)
                    {
                        throw new ValidationException("Role names must be provided.");
                    }

                    // Create user entity
                    var user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        isApproved=model.isApproved,
                        isApprovedByAdmin= model.isApprovedByAdmin,
                        isEmployee = model.isEmployee,
                        BranchId= model.BranchId,
                        CompanyId= model.CompanyId,
                        
                    };

                    // Check if all roles exist
                    foreach (var role in model.Roles)
                    {
                        if (await _roleManager.FindByNameAsync(role) == null)
                        {
                            throw new ValidationException("One or more roles are invalid.");
                        }
                    }

                    // Create user
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return (false, null, string.Join("; ", result.Errors.Select(e => e.Description)));
                    }

                    // Add user to roles
                    var addUserRole = await _userManager.AddToRolesAsync(user, model.Roles);
                    if (!addUserRole.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return (false, null, string.Join("; ", addUserRole.Errors.Select(e => e.Description)));
                    }

                    // Conditional customer creation
                    if (model.isApproved == true && model.isEmployee == false)
                    {
                        var customer = new CustomerDTOs
                        {
                            CustomerName = $"{model.FirstName} {model.LastName}",
                            ContactName = " ",
                            ContactTitle = " ",
                            Address = "Default Address",
                            City = " ",
                            Region = " ",
                            PostalCode = " ",
                            Country = " ",
                            Phone = " ",
                            Fax = " ",
                            Email = model.Email, // Ensure this is provided as it's required
                            PasswordHash = model.Password, // Hash the password
                            DateOfBirth = DateTime.Now, // Adjust as needed
                            MedicalHistory = " ",
                        };

                        var customerResult = await _customer.CreateAsync(customer);
                        if (!customerResult)
                        {
                            await transaction.RollbackAsync();
                            return (false, null, "Customer registration failed.");
                        }
                    }

                    // Commit the transaction
                    await transaction.CommitAsync();
                    return (true, user.Id, null);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, null, $"An error occurred during registration: {ex.Message}");
                }
            }
        }


    }
}
