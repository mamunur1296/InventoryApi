using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LeaveRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to LeaveRepository here

    }
}
