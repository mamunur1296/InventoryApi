using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class SubMenuRepository : Repository<SubMenu>, ISubMenuRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SubMenuRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to SubMenuRepository here

    }

}
