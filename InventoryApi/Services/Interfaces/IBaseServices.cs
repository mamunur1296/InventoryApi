using InventoryApi.DTOs;

namespace InventoryApi.Services.Interfaces
{
    public interface IBaseServices<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(string id,T entity);
        Task<bool> DeleteAsync(string id);
    }
}
