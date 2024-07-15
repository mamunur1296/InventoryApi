using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CompanyRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to CompanyRepository here

    }
}
