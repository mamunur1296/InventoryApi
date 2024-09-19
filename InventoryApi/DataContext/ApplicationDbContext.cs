using InventoryApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<UnitChild> UnitChilds { get; set; }
        public DbSet<Branch> BranchItems { get; set; }
        public DbSet<UnitMaster> UnitMasterItems { get; set; }


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
            
            builder.Entity<Product>()
                .HasOne(p => p.UnitMaster)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.UnitChild)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UnitChildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UnitChild>()
                .HasOne(u => u.UnitMaster)
                .WithMany(um => um.UnitChildrens)
                .HasForeignKey(u => u.UnitMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Warehouse>()
                .HasOne(w => w.Branch)
                .WithMany(b => b.Warehouses)
                .HasForeignKey(w => w.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Stock>()
                .HasOne(s => s.Warehouse)
                .WithMany(w => w.Stocks)
                .HasForeignKey(s => s.WarehouseID)
                .OnDelete(DeleteBehavior.Restrict);
           
        }
    }

}
