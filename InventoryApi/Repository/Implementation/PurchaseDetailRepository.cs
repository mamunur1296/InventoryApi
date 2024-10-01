using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class PurchaseDetailRepository : Repository<PurchaseDetail>, IPurchaseDetailRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PurchaseDetailRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to PurchaseDetailRepository here

    }
}
