using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class ShipperRepository : Repository<Shipper>, IShipperRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ShipperRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to ShipperRepository here

    }
}
