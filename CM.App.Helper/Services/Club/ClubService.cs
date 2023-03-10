using System.Net.Http.Json;
using CM.App.Helper.State;
using CM.Model.Dto;
using CM.Model.General;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CM.App.Helper.Services;

public class ClubService : BaseService<ClubDto>, IClubService
{
    public ClubService(
        NavigationManager navigation,
        IGeneralStateProvider state,
        HttpClient client,
        ISnackbar snackbar
    ) : base(navigation, state, client, snackbar)
    {
        BaseUrl = "api/club/v1";
    }

    public async Task<IEnumerable<ClubDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/autocomplete"
                + $"?filter={parameters.Filter}"
                + $"&Count={parameters.Count}"
        );
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ClubDto>>();
            return result ?? new List<ClubDto>();
        }

        return new List<ClubDto>();
    }
}
