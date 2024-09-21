using InventoryUi.DTOs;
using InventoryUi.Services.Interface;
using Newtonsoft.Json;

public class ClientServices<T> : IClientServices<T> where T : class
{
    private readonly IHttpService _httpService;

    public ClientServices(IHttpService httpService)
    {
        _httpService = httpService;
    }

    private async Task<TResult> SendRequestAsync<TResult>(string endpoint, ApiType apiType, object data = null, bool includeToken = true)
    {
        var request = new ClientRequest
        {
            Url = Helper.BaseUrl + endpoint,
            ApiType = apiType,
            ContentType = ContentType.Json,
            Data = data
        };

        string response = await _httpService.SendData(request, token: includeToken);
        return JsonConvert.DeserializeObject<TResult>(response);
    }


    public Task<ResponseDto<object>> DeleteClientAsync(string endpoint)
    {
        return SendRequestAsync<ResponseDto<object>>(endpoint, ApiType.Delete);
    }

    public Task<ResponseDto<IEnumerable<T>>> GetAllClientsAsync(string endpoint)
    {
        return SendRequestAsync<ResponseDto<IEnumerable<T>>>(endpoint, ApiType.Get);
    }

    public Task<ResponseDto<T>> GetClientByIdAsync(string endpoint)
    {
        return SendRequestAsync<ResponseDto<T>>(endpoint, ApiType.Get);
    }

    public Task<ResponseDto<object>> PostClientAsync(string endpoint, T client)
    {
        return SendRequestAsync<ResponseDto<object>>(endpoint, ApiType.Post, client);
    }

    public Task<ResponseDto<object>> UpdateClientAsync(string endpoint, T client)
    {
        return SendRequestAsync<ResponseDto<object>>(endpoint, ApiType.Put, client);
    }

    public Task<ResponseDto<LoginResponseDto>> Login(string endpoint, T client)
    {
        return SendRequestAsync<ResponseDto<LoginResponseDto>>(endpoint, ApiType.Post, client, includeToken: false);
    }
}
