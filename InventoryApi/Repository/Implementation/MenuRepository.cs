using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MenuRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to MenuRepository here

    }

}
