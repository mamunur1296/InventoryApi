using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class MenuRoleRepository : Repository<MenuRole>, IMenuRoleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MenuRoleRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to MenuRoleRepository here

    }

}
