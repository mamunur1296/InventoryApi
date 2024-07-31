using InventoryApi.DataContext;
using InventoryApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Seed roles
        string[] roleNames = { "SuperAdmin", "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Create the roles if they do not exist
                var roleResult = await roleManager.CreateAsync(new ApplicationRole { Name = roleName, NormalizedName = roleName.ToUpper() });
                if (!roleResult.Succeeded)
                {
                    // Handle role creation failure
                    throw new Exception($"Failed to create role {roleName}");
                }
            }
        }

        // Seed menus
        if (!context.Menus.Any() && !context.SubMenus.Any())
        {
            var menus = new Menu[]
            {
                new Menu { Name = "Company" },
                new Menu { Name = "Product" },
                new Menu { Name = "User" },
                new Menu { Name = "Role" }
            };

            foreach (Menu menu in menus)
            {
                context.Menus.Add(menu);
            }

            context.SaveChanges();
        }

        // Seed a default admin user
        var adminUser = new ApplicationUser
        {
            UserName = "admin@123",
            Email = "admin@Gmail.com",
            FirstName = "Mamunur Rudhid",
            LastName = "Admin",
            PhoneNumber = "01767988385",
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
