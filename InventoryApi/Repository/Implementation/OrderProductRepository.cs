using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public OrderProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to OrderProductRepository here

    }

}
