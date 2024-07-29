using InventoryApi.DataContext;
using InventoryApi.Repository.Implementation;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.UnitOfWork
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ICompanyRepository companyRepository { get; private set; }

        public IDeliveryAddressRepository deliveryAddressRepository { get; private set; }

        public IWarehouseRepository warehouseRepository { get; private set; }

        public IProductRepository productRepository { get; private set; }

        public ICategoryRepository categoryRepository { get; private set; }

        public IOrderRepository orderRepository { get; private set; }
        public IOrderProductRepository orderProductRepository { get; private set; }

        public IMenuRepository menuRepository { get; private set; }

        public ISubMenuRepository subMenuRepository { get; private set; }

        public IMenuRoleRepository menuRoleRepository { get; private set; }

        public ISubMenuRoleRepository subMenuRoleRepository { get; private set; }

        public UnitOfWorkRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            companyRepository = new CompanyRepository(applicationDbContext);
            deliveryAddressRepository = new DeliveryAddressRepository(applicationDbContext);
            warehouseRepository = new WarehouseRepository(applicationDbContext);
            productRepository = new ProductRepository(applicationDbContext);
            categoryRepository = new CategoryRepository(applicationDbContext);
            orderRepository = new OrderRepository(applicationDbContext);
            menuRepository = new MenuRepository(applicationDbContext);
            orderProductRepository = new OrderProductRepository(applicationDbContext);
            subMenuRepository = new SubMenuRepository(applicationDbContext);
            menuRoleRepository = new MenuRoleRepository(applicationDbContext);
            subMenuRoleRepository = new SubMenuRoleRepository(applicationDbContext);

        }

        public async Task SaveAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
