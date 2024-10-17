using InventoryApi.Entities;
using Microsoft.AspNetCore.Identity;


public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed roles
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Create the roles if they do not exist
                var roleResult = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!roleResult.Succeeded)
                {
                    // Handle role creation failure
                    throw new Exception($"Failed to create role {roleName}");
                }
            }
        }

        // Seed a default admin user
        var adminUser = new ApplicationUser
        {
            UserName = "admin@123",
            Email = "admin@gmail.com", 
            FirstName = "Super", 
            LastName = "Admin",
            PhoneNumber = "01711223344",
            isApproved = true,  
            isApprovedByAdmin = false,
            isEmployee = false,
        };

        string adminPassword = "admin@123";

        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null)
        {
            var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdminUser.Succeeded)
            {
                // Assign the "Admin" role to the admin user
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
