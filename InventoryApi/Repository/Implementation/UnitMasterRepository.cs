using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class UnitMasterRepository : Repository<UnitMaster>, IUnitMasterRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitMasterRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to UnitMasterRepository here

    }
}
