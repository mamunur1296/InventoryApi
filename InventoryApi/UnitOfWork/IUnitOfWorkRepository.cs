using InventoryApi.Repository.Interfaces;

namespace InventoryApi.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        ICompanyRepository companyRepository { get; }
        IDeliveryAddressRepository deliveryAddressRepository { get; }
        IWarehouseRepository warehouseRepository { get; }
        IProductRepository productRepository { get; }
        ICategoryRepository categoryRepository { get; }
        IOrderRepository orderRepository { get; }
        IOrderProductRepository orderProductRepository { get; }
        IMenuRepository menuRepository { get; }
        ISubMenuRepository subMenuRepository { get; }
        IMenuRoleRepository menuRoleRepository { get; }
        ISubMenuRoleRepository subMenuRoleRepository { get; } //
        ISupplierRepository supplierRepository { get; }
        IStockRepository stockRepository { get; }
        IShoppingCartRepository shoppingCartRepository { get; }
        IShipperRepository shipperRepository { get; }
        IReviewRepository reviewRepository { get; }
        IPrescriptionRepository prescriptionRepository { get; }
        IPaymentRepository paymentRepository { get; }
        IOrderDetailRepository orderDetailRepository { get; }
        IEmployeeRepository employeeRepository { get; } 
        ICustomerRepository customerRepository { get; }
        ICartItemRepository cartItemRepository { get; }
        IBranchRepository branchRepository { get; }
        IUnitChildRepository unitChildRepository { get; }
        IUnitMasterRepository unitMasterRepository { get; }
        IPurchaseDetailRepository purchaseDetailRepository { get; }
        IPurchaseRepository purchaseRepository { get; }

        Task SaveAsync();
    }
}
