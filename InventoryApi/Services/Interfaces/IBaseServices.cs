using InventoryApi.DTOs;

namespace InventoryApi.Services.Interfaces
{
    public interface IBaseServices<T> where T : class
    {
        Task<ResponseDTOs<IEnumerable<T>>> GetAllAsync();
        Task<ResponseDTOs<T>> GetByIdAsync(string id);
        Task<ResponseDTOs<string>> CreateAsync(T entity);
        Task<ResponseDTOs<string>> UpdateAsync(T entity);
        Task<ResponseDTOs<string>> DeleteAsync(string id);
    }
}
