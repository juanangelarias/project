using System.Net.Http.Json;
using CM.App.Helper.State;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Tools;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CM.App.Helper.Services;

public class CountryService : BaseService<CountryDto>, ICountryService
{
    public CountryService(NavigationManager navigation, IGeneralStateProvider state, HttpClient client,
        ISnackbar snackbar)
        : base(navigation, state, client, snackbar)
    {
        BaseUrl = "api/country/v1";
    }

    public async Task<IEnumerable<CountryDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/autocomplete" +
                                                             $"?filter={parameters.Filter}" +
                                                             $"&Count={parameters.Count}");
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<CountryDto>>();
            return result ?? new List<CountryDto>();
        }

        return new List<CountryDto>();
    }
}