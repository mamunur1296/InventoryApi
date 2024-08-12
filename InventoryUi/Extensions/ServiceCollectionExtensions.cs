using InventoryUi.DTOs;
using InventoryUi.Models;
using InventoryUi.Services.Implemettions;
using InventoryUi.Services.Interface;
using InventoryUi.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InventoryUi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Reading the BaseUrl value from configuration
            var baseUrl = configuration["BaseUrl:AuthenticationAPI"];
            // Assign it to Helper.BaseUrl if Helper is a static class
            Helper.BaseUrl = baseUrl;

            services.AddHttpClient();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFileUploader, FileUploader>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IUtilityHelper, UtilityHelper>();
            services.AddScoped<IClientServices<ChangePassword>, ClientServices<ChangePassword>>();
            services.AddScoped<IClientServices<User>, ClientServices<User>>();
            services.AddScoped<IClientServices<Register>, ClientServices<Register>>();
            services.AddScoped<IClientServices<Login>, ClientServices<Login>>();
            services.AddScoped<IClientServices<Roles>, ClientServices<Roles>>();
            services.AddScoped<IClientServices<Company>, ClientServices<Company>>();
            services.AddScoped<IClientServices<Warehouse>, ClientServices<Warehouse>>();
            services.AddScoped<IClientServices<Supplier>, ClientServices<Supplier>>();
            services.AddScoped<IClientServices<SubMenuRole>, ClientServices<SubMenuRole>>();
            services.AddScoped<IClientServices<SubMenu>, ClientServices<SubMenu>>();
            services.AddScoped<IClientServices<Stock>, ClientServices<Stock>>();
            services.AddScoped<IClientServices<ShoppingCart>, ClientServices<ShoppingCart>>();
            services.AddScoped<IClientServices<Shipper>, ClientServices<Shipper>>();
            services.AddScoped<IClientServices<Review>, ClientServices<Review>>();
            services.AddScoped<IClientServices<Product>, ClientServices<Product>>();
            services.AddScoped<IClientServices<Prescription>, ClientServices<Prescription>>();
            services.AddScoped<IClientServices<Payment>, ClientServices<Payment>>();
            services.AddScoped<IClientServices<OrderProduct>, ClientServices<OrderProduct>>();
            services.AddScoped<IClientServices<OrderDetail>, ClientServices<OrderDetail>>();
            services.AddScoped<IClientServices<Order>, ClientServices<Order>>();
            services.AddScoped<IClientServices<MenuRole>, ClientServices<MenuRole>>();
            services.AddScoped<IClientServices<Menu>, ClientServices<Menu>>();
            services.AddScoped<IClientServices<Employee>, ClientServices<Employee>>();
            services.AddScoped<IClientServices<DeliveryAddress>, ClientServices<DeliveryAddress>>();
            services.AddScoped<IClientServices<Customer>, ClientServices<Customer>>();
            services.AddScoped<IClientServices<Category>, ClientServices<Category>>();
            services.AddScoped<IClientServices<CartItem>, ClientServices<CartItem>>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                    options.ReturnUrlParameter = "ReturnUrl";
                });

            services.AddSession();

            return services;
        }
    }
}
