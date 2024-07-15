using InventoryApi.DataContext;
using InventoryApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _dbSet = _applicationDbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
