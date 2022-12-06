using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CM.App.Helper.State;
using CM.Model.Dto;
using CM.Model.General;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CM.App.Helper.Services;

public class ConferenceService : BaseService<ConferenceDto>, IConferenceService
{
    public ConferenceService(
        NavigationManager navigation,
        IGeneralStateProvider state,
        HttpClient client,
        ISnackbar snackbar
    ) : base(navigation, state, client, snackbar)
    {
        BaseUrl = "api/conference/v1";
    }

    public async Task<IEnumerable<ConferenceDto>> Autocomplete(AutoCompleteParams parameters)
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
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ConferenceDto>>();
            return result ?? new List<ConferenceDto>();
        }

        return new List<ConferenceDto>();
    }

    public async Task<PagedResponse<InscriptionDto>> GetInscriptions(InscriptionQueryParams parameters)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/inscriptions" +
            $"?conferenceId={parameters.ConferenceId}" +
            $"&filter={parameters.Filter}" +
            $"&PageSize={parameters.PageSize}" +
            $"&PageIndex={parameters.PageIndex}" +
            $"&Sort={parameters.Sort}" +
            $"&Descending={parameters.Descending}" +
            $"&Expand={parameters.Expand}");

        var response = await GetResponse(request);

        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<PagedResponse<InscriptionDto>>();
            return result ?? new PagedResponse<InscriptionDto>(new List<InscriptionDto>(), 0);
        }

        return new PagedResponse<InscriptionDto>(new List<InscriptionDto>(), 0);
    }

    public async Task<IEnumerable<ProductDto>> GetProducts(long conferenceId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/{conferenceId}/products");

        var response = await GetResponse(request);

        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            return result ?? new List<ProductDto>();
        }

        return new List<ProductDto>();
    }

    public async Task<IEnumerable<ProgramDto>> GetPrograms(long conferenceId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/{conferenceId}/programs");

        var response = await GetResponse(request);

        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProgramDto>>();
            return result ?? new List<ProgramDto>();
        }

        return new List<ProgramDto>();
    }

    public async Task<IEnumerable<ConferencePaymentMethodDto>> GetPaymentMethods(long conferenceId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/{conferenceId}/paymentMethods");

        var response = await GetResponse(request);

        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ConferencePaymentMethodDto>>();
            return result ?? new List<ConferencePaymentMethodDto>();
        }

        return new List<ConferencePaymentMethodDto>();
    }

    public async Task<ProgramDto> CreateProgram(ProgramDto program)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/program")
        {
            Content = new StringContent(JsonSerializer.Serialize(program), Encoding.UTF8, "application/json")
        };
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<ProgramDto>();
            return result ?? new ProgramDto();
        }
        
        return new ProgramDto();
    }

    public async Task<IEnumerable<ConferencePaymentMethodDto>> SetPaymentMethods(List<ConferencePaymentMethodDto> paymentMethods)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/paymentMethods")
        {
            Content = new StringContent(JsonSerializer.Serialize(paymentMethods), Encoding.UTF8, "application/json")
        };

        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<ConferencePaymentMethodDto>>();
            return result ?? new List<ConferencePaymentMethodDto>();
        }   
        return new List<ConferencePaymentMethodDto>();
    }

    public async Task<ProgramDto> UpdateProgram(ProgramDto program)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/program")
        {
            Content = new StringContent(JsonSerializer.Serialize(program), Encoding.UTF8, "application/json")
        };
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<ProgramDto>();
            return result ?? new ProgramDto();
        }
        
        return new ProgramDto();
    }

    public async Task DeleteProgram(long programId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/program/{programId}");
        await GetResponse(request);
    }
}