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
        private readonly IBaseServices<EmployeeDTOs> _employee;
        private readonly ApplicationDbContext _context;
        private readonly IUserContextService _userContextService;

        public HelperServicess(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IBaseServices<CustomerDTOs> customer,
            ApplicationDbContext context,
            IBaseServices<EmployeeDTOs> employee
,
            IUserContextService userContextService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _customer = customer;
            _context = context;
            _employee = employee;
            _userContextService = userContextService;
        }

        public async Task<(bool isSucceed, string CustomerId, string errorMessage)> CreateCustomerByAdmin(string id)
        {
            // Begin a database transaction
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the provided ID is null
                    if (string.IsNullOrEmpty(id))
                    {
                        throw new NotFoundException("User ID not found.");
                    }

                    // Check if the user exists
                    var existingUser = await _userManager.FindByIdAsync(id);
                    if (existingUser == null)
                    {
                        throw new NotFoundException("User does not exist.");
                    }

                    // Create the customer DTO
                    var customer = new CustomerDTOs
                    {
                        CustomerName = $"{existingUser.FirstName} {existingUser.LastName}",
                        ContactName = string.Empty,
                        ContactTitle = string.Empty,
                        Address = existingUser.Address,
                        City = string.Empty,
                        Region = string.Empty,
                        PostalCode = string.Empty,
                        Country = string.Empty,
                        Phone = existingUser.PhoneNumber,
                        Fax = string.Empty,
                        Email = existingUser.Email, // Ensure email is provided
                        PasswordHash = existingUser.PasswordHash, // Use hashed password
                        DateOfBirth = DateTime.Now, // Adjust as needed
                        MedicalHistory = string.Empty,
                        Id = existingUser.Id,
                    };

                    // Attempt to create the customer
                    var customerResult = await _customer.CreateAsync(customer);
                    if (!customerResult)
                    {
                        await transaction.RollbackAsync();
                        return (false, null, "Failed to register the customer.");
                    }

                    // Update the user details
                    existingUser.isApprovedByAdmin = true; // Mark as approved by admin
                    existingUser.isEmployee = false; // If the user is not an employee

                    var updateUserResult = await _userManager.UpdateAsync(existingUser);
                    if (!updateUserResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return (false, null, "Failed to update user information.");
                    }

                    // Commit the transaction
                    await transaction.CommitAsync();
                    return (true, customer.Id, null);
                }
                catch (Exception ex)
                {
                    // Rollback in case of an error and return the error message
                    await transaction.RollbackAsync();
                    throw new ValidationException($"An error occurred during customer registration: {ex.Message}");
                }
            }
        }

        public async Task<(bool isSucceed, string CustomerId, string errorMessage)> CreateEmployeeByAdmin(string id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the provided ID is null
                    if (string.IsNullOrEmpty(id))
                    {
                        throw new NotFoundException("User ID not found.");
                    }

                    // Check if the user exists
                    var existingUser = await _userManager.FindByIdAsync(id);
                    if (existingUser == null)
                    {
                        throw new NotFoundException("User does not exist.");
                    }

                    // Create the customer DTO
                    var newEmployee = new EmployeeDTOs
                    {
                        CreationDate = DateTime.Now, // Set CreationDate here
                        FirstName = existingUser.FirstName.Trim(),
                        LastName = existingUser.LastName.Trim(),
                        Title = null,
                        TitleOfCourtesy = null,
                        BirthDate = null,
                        HireDate = DateTime.Now,
                        Address = null,
                        City = null,
                        Region = null,
                        PostalCode = null,
                        Country = null,
                        HomePhone = existingUser.PhoneNumber,
                        Extension = null,
                        Photo = null,
                        Notes = null,
                        ReportsTo = null,
                        PhotoPath = null,
                        ManagerId = null,
                        Salary = 100,
                        UserId = existingUser.Id,
                        Id = existingUser.Id,
                        CompanyId = existingUser.CompanyId,
                        BranchId = existingUser.BranchId,
                    };

                    // Attempt to create the customer
                    var employeeResult = await _employee.CreateAsync(newEmployee);
                    if (!employeeResult)
                    {
                        await transaction.RollbackAsync();
                        return (false, null, "Failed to register the Employee.");
                    }

                    // Update the user details
                    existingUser.isApprovedByAdmin = true; // Mark as approved by admin
                    existingUser.isEmployee = false; // If the user is not an employee

                    var updateUserResult = await _userManager.UpdateAsync(existingUser);
                    if (!updateUserResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return (false, null, "Failed to update user information.");
                    }

                    // Commit the transaction
                    await transaction.CommitAsync();
                    return (true, newEmployee?.UserId, null);
                }
                catch (Exception ex)
                {
                    // Rollback in case of an error and return the error message
                    await transaction.RollbackAsync();
                    throw new ValidationException($"An error occurred during Employee registration: {ex.Message}");
                }
            }
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
                        return (false, null, "Role names must be provided.");
                    }

                    // Create user entity
                    var user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        isApproved = model.isApproved,
                        isApprovedByAdmin = model.isApprovedByAdmin,
                        isEmployee = model.isEmployee,
                        BranchId = model.BranchId,
                        CompanyId = model.CompanyId,
                    };

                    // Check if all roles exist
                    foreach (var role in model.Roles)
                    {
                        if (await _roleManager.FindByNameAsync(role) == null)
                        {
                            return (false, null, "One or more roles are invalid.");
                        }
                    }

                    // Create user
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        // Create an error message from the result errors
                        string errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                        return (false, null, errorMessage);
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
                            ContactName = null,
                            ContactTitle = null,
                            Address = null,
                            City = null,
                            Region = null,
                            PostalCode = null,
                            Country = null,
                            Phone = model.PhoneNumber,
                            Fax = null,
                            Email = model.Email, // Ensure this is provided as it's required
                            PasswordHash = model.Password, // Hash the password
                            DateOfBirth = DateTime.Now, // Adjust as needed
                            MedicalHistory = null,
                            Id = user.Id,
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
