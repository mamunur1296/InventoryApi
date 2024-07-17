using InventoryUi.DTOs;

namespace InventoryUi.Services.Interface
{
    public interface IClientServices<T> where T : class
    {
        Task<ResponseDto<IEnumerable<T>>> GetAllClientsAsync(string endpoint);
        Task<ResponseDto<T>> GetClientByIdAsync(string endpoint);
        Task<ResponseDto<object>> PostClientAsync(string endpoint, T data);
        Task<ResponseDto<object>> UpdateClientAsync(string endpoint, T data);
        Task<ResponseDto<object>> DeleteClientAsync(string endpoint);
        Task<ResponseDto<LoginResponseDto>> Login(string endpoint, T data);
    }
}


