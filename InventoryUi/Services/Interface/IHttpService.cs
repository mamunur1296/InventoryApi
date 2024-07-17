using InventoryUi.DTOs;

namespace InventoryUi.Services.Interface
{
    public interface IHttpService
    {
        Task<string> SendData(ClientRequest requestData, bool token = true);
    }
}
