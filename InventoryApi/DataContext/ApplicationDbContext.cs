using InventoryApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<DeliveryAddress>? deliveryAddresses { get; set; }
        public DbSet<Company>? companies { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MenuRole>().HasKey(mr => new { mr.MenuId, mr.RoleId });
            builder.Entity<MenuRole>().HasOne(mr => mr.Menu).WithMany(m => m.MenuRoles).HasForeignKey(mr => mr.MenuId);
            builder.Entity<MenuRole>().HasOne(mr => mr.Role).WithMany().HasForeignKey(mr => mr.RoleId);
            builder.Entity<SubMenuRole>().HasKey(smr => new { smr.SubMenuId, smr.RoleId });
            builder.Entity<SubMenuRole>().HasOne(smr => smr.SubMenu).WithMany(sm => sm.SubMenuRoles).HasForeignKey(smr => smr.SubMenuId);
            builder.Entity<SubMenuRole>().HasOne(smr => smr.Role).WithMany().HasForeignKey(smr => smr.RoleId);
            builder.Entity<Order>().Property(o => o.Freight).HasPrecision(18, 2);
        }
    }

}
