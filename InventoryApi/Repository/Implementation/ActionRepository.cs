using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class ActionRepository : Repository<ActionName>, IActionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ActionRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to CategoryRepository here

    }
}
