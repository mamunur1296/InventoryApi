using InventoryApi.Controllers;
using InventoryApi.DataContext;
using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Services.Implementation;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProjectApi
{
    public static class Infrastructiure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("dbcs"));
            });
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false; // For special character
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
            });
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<RoleManager<ApplicationRole>>();
            services.AddScoped<IBaseServices<CompanyDTOs>, CompanyService>();
            services.AddScoped<IBaseServices<DeliveryAddressDTOs>, DeliveryAddressService>();
            services.AddScoped<IBaseServices<WarehouseDTOs>, WarehouseService>();
            services.AddScoped<IBaseServices<ProductDTOs>, ProductService>();
            services.AddScoped<IBaseServices<CategoryDTOs>, CategoryService>();
            services.AddScoped<IBaseServices<OrderDTOs>, OrderService>();
            services.AddScoped<IBaseServices<OrderProductDTOs>, OrderProductService>();
            services.AddScoped<IBaseServices<MenuDTOs>, MenuService>();
            services.AddScoped<IBaseServices<SubMenuDTOs>, SubMenuService>();
            services.AddScoped<IBaseServices<MenuRoleDTOs>, MenuRoleService>();
            services.AddScoped<IBaseServices<SubMenuRoleDTOs>, SubMenuRoleService>();
            return services;
        }
    }
}
