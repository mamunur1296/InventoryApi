using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ShiftRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to ShiftRepository here

    }
}
