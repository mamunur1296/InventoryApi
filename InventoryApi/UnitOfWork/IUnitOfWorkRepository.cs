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
        ISubMenuRoleRepository subMenuRoleRepository { get; }
        Task SaveAsync();
    }
}
