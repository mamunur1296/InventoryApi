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
        public UnitOfWorkRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            companyRepository = new CompanyRepository(applicationDbContext);
            deliveryAddressRepository = new DeliveryAddressRepository(applicationDbContext);

        }

        public async Task SaveAsync()
        {
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
