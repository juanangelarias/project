using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CM.App.Helper.State;
using CM.Model.Dto.Base;
using CM.Model.General;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CM.App.Helper.Services;

public abstract class BaseService<T>: IBaseService<T>
    where T: BaseDto
{
    private readonly NavigationManager _navigation;
    private readonly IGeneralStateProvider _state;
    private readonly HttpClient _client;
    private readonly ISnackbar _snackbar;
    protected string BaseUrl { get; set; } = "";

    protected BaseService(NavigationManager navigation, IGeneralStateProvider state, HttpClient client, ISnackbar snackbar)
    {
        _navigation = navigation;
        _state = state;
        _client = client;
        _snackbar = snackbar;
    }
    
    public async Task<List<T>> Get()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<List<T>>();
            return result ?? new List<T>();
        }

        return new List<T>();
    }

    public async Task<PagedResponse<T>> GetPage(QueryParams parameters)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/getPage" +
                                                             $"?filter={parameters.Filter}" +
                                                             $"&PageSize={parameters.PageSize}" +
                                                             $"&PageIndex={parameters.PageIndex}" +
                                                             $"&Sort={parameters.Sort}" +
                                                             $"&Descending={parameters.Descending}" +
                                                             $"&Expand={parameters.Expand}");
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<PagedResponse<T>>();
            return result ?? new PagedResponse<T>(new List<T>(), 0);
        }

        return new PagedResponse<T>(new List<T>(), 0);
    }

    public async Task<T?> Get(long id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{id}");
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<T>();
            return result;
        }

        return null;
    }

    public async Task<T?> Create(T dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<T>();
            return result;
        }

        return null;
    }

    public async Task<T?> Update(T dto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}")
        {
            Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json")
        };
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<T>();
            return result;
        }

        return null;
    }

    public async Task Delete(long id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/{id}");
        await GetResponse(request);
    }

    public async Task<HttpResponseMessage?> GetResponse(HttpRequestMessage request)
    {
        // var isApiUrl = Convert.ToBoolean(request.RequestUri?.IsAbsoluteUri);
        if (_state.Token != "") // && isApiUrl)
            request.Headers.Add("Authorization", $"Bearer {_state.Token}");

        var response = await _client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navigation.NavigateTo("/logout");
            return default;
        }

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var message = await response.Content.ReadAsStringAsync();

        _snackbar.Add(message, Severity.Error);
        return null;
    }
}