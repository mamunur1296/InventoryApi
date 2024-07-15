using InventoryApi.Entities;
using Microsoft.AspNetCore.Identity;


public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Seed roles
        string[] roleNames = { "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Create the roles if they do not exist
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        //// Seed a default admin user
        //var adminUser = new ApplicationUser
        //{
        //    UserName = "admin@admin.com",
        //    Email = "admin@admin.com",
        //    EmailConfirmed = true
        //};

        //string adminPassword = "Admin@123";

        //var user = await userManager.FindByEmailAsync(adminUser.Email);

        //if (user == null)
        //{
        //    var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
        //    if (createAdminUser.Succeeded)
        //    {
        //        // Assign the "Admin" role to the admin user
        //        await userManager.AddToRoleAsync(adminUser, "Admin");
        //    }
        //}
    }
}
