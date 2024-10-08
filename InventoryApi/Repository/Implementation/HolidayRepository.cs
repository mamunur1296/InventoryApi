using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HolidayRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to HolidayRepository here

    }
}
