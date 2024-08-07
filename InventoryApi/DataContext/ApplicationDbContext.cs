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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DeliveryAddress>().HasKey(op => new { op.Id,op.UserId });
            builder.Entity<OrderProduct>().HasKey(op => new { op.Id, op.ProductId });
            builder.Entity<OrderProduct>().HasOne(op => op.Order).WithMany(op => op.OrderProducts).HasForeignKey(op => op.Id);
            builder.Entity<OrderProduct>().HasOne(op => op.Product).WithMany(p => p.OrderProducts).HasForeignKey(op => op.ProductId);
            builder.Entity<MenuRole>().HasKey(mr => new { mr.MenuId, mr.RoleId });
            builder.Entity<MenuRole>().HasOne(mr => mr.Menu).WithMany(m => m.MenuRoles).HasForeignKey(mr => mr.MenuId);
            builder.Entity<MenuRole>().HasOne(mr => mr.Role).WithMany().HasForeignKey(mr => mr.RoleId);
            builder.Entity<SubMenuRole>().HasKey(smr => new { smr.SubMenuId, smr.RoleId });
            builder.Entity<SubMenuRole>().HasOne(smr => smr.SubMenu).WithMany(sm => sm.SubMenuRoles).HasForeignKey(smr => smr.SubMenuId);
            builder.Entity<SubMenuRole>().HasOne(smr => smr.Role).WithMany().HasForeignKey(smr => smr.RoleId);


            // Configure Company and Warehouse relationship
            builder.Entity<Company>()
                .HasMany(c => c.Warehouses)
                .WithOne(w => w.Company)
                .HasForeignKey(w => w.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define a non-unique index on CompanyId
            builder.Entity<Warehouse>()
                   .HasIndex(w => w.CompanyId);
        }
    }

}
