using InventoryApi.Repository.Interfaces;

namespace InventoryApi.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        ICompanyRepository companyRepository { get; }
        IDeliveryAddressRepository deliveryAddressRepository { get; }
        Task SaveAsync();
    }
}
