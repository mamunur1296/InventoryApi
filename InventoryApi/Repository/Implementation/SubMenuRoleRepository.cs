using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class SubMenuRoleRepository : Repository<SubMenuRole>, ISubMenuRoleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SubMenuRoleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to SubMenuRoleRepository here

    }

}
