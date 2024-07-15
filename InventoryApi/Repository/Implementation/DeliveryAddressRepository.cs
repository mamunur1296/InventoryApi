using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class DeliveryAddressRepository : Repository<DeliveryAddress>, IDeliveryAddressRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public DeliveryAddressRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to DeliveryAddressRepository here

    }
}
