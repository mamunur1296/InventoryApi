using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class UnitChildRepository : Repository<UnitChild>, IUnitChildRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitChildRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to UnitChildRepository here

    }
}
