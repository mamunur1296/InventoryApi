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

        public ISubMenuRoleRepository subMenuRoleRepository { get; private set; }//

        public ISupplierRepository supplierRepository { get; private set; }

        public IStockRepository stockRepository { get; private set; }

        public IShoppingCartRepository shoppingCartRepository { get; private set; }

        public IShipperRepository shipperRepository { get; private set; }

        public IReviewRepository reviewRepository { get; private set; }

        public IPrescriptionRepository prescriptionRepository { get; private set; }

        public IPaymentRepository paymentRepository { get; private set; }

        public IOrderDetailRepository orderDetailRepository { get; private set; }

        public IEmployeeRepository employeeRepository { get; private set; }

        public ICustomerRepository customerRepository { get; private set; }

        public ICartItemRepository cartItemRepository { get; private set; }

        public IBranchRepository branchRepository { get; private set; }

        public IUnitChildRepository unitChildRepository { get; private set; }

        public IUnitMasterRepository unitMasterRepository { get; private set; }

        public IPurchaseDetailRepository purchaseDetailRepository { get; private set; }

        public IPurchaseRepository purchaseRepository { get; private set; }

        public IAttendanceRepository attendanceRepository { get; private set; }

        public IDepartmentRepository departmentRepository { get; private set; }

        public IHolidayRepository holidayRepository { get; private set; }

        public ILeaveRepository leaveRepository { get; private set; }

        public IPayrollRepository payrollRepository { get; private set; }

        public IShiftRepository shiftRepository { get; private set; }

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
            subMenuRoleRepository = new SubMenuRoleRepository(applicationDbContext);//
            supplierRepository = new SupplierRepository(applicationDbContext);
            stockRepository = new StockRepository(applicationDbContext);
            shoppingCartRepository = new ShoppingCartRepository(applicationDbContext);
            shipperRepository = new ShipperRepository(applicationDbContext);
            reviewRepository = new ReviewRepository(applicationDbContext);
            prescriptionRepository = new PrescriptionRepository(applicationDbContext);
            paymentRepository = new PaymentRepository(applicationDbContext);
            orderDetailRepository = new OrderDetailRepository(applicationDbContext);
            employeeRepository = new EmployeeRepository(applicationDbContext);
            customerRepository = new CustomerRepository(applicationDbContext);
            cartItemRepository = new CartItemRepository(applicationDbContext);
            branchRepository = new BranchRepository(applicationDbContext);
            unitChildRepository = new UnitChildRepository(applicationDbContext);
            unitMasterRepository = new UnitMasterRepository(applicationDbContext);
            purchaseDetailRepository= new PurchaseDetailRepository(applicationDbContext);
            purchaseRepository= new PurchaseRepository(applicationDbContext);
            attendanceRepository = new AttendanceRepository(applicationDbContext);
            departmentRepository = new DepartmentRepository(applicationDbContext);
            holidayRepository = new HolidayRepository(applicationDbContext);
            leaveRepository = new LeaveRepository(applicationDbContext);
            payrollRepository = new PayrollRepository(applicationDbContext);
            shiftRepository = new ShiftRepository(applicationDbContext);
        }

        public async Task SaveAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
