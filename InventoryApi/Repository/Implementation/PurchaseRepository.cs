using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PurchaseRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to PurchaseRepository here

    }
}
