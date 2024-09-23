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


    public async Task<ResponseDto<object>> DeleteClientAsync(string endpoint)
    {
        return  await SendRequestAsync<ResponseDto<object>>(endpoint, ApiType.Delete);
    }

    public async Task<ResponseDto<IEnumerable<T>>> GetAllClientsAsync(string endpoint)
    {
        return await SendRequestAsync<ResponseDto<IEnumerable<T>>>(endpoint, ApiType.Get);
    }

    public async Task<ResponseDto<T>> GetClientByIdAsync(string endpoint)
    {
        return await SendRequestAsync<ResponseDto<T>>(endpoint, ApiType.Get);
    }

    public async Task<ResponseDto<object>> PostClientAsync(string endpoint, T client)
    {
        return await SendRequestAsync<ResponseDto<object>>(endpoint, ApiType.Post, client);
    }

    public async Task<ResponseDto<object>> UpdateClientAsync(string endpoint, T client)
    {
        return await SendRequestAsync<ResponseDto<object>>(endpoint, ApiType.Put, client);
    }

    public async Task<ResponseDto<LoginResponseDto>> Login(string endpoint, T client)
    {
        return await SendRequestAsync<ResponseDto<LoginResponseDto>>(endpoint, ApiType.Post, client, includeToken: false);
    }
}
