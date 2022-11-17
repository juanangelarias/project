using CM.App.Helper.Model.Enum;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface ICountryStateProvider
{
    List<CountryDto> CountryList { get; set; }
    CountryDto? SelectedCountry { get; set; }
    TableData<CountryDto> CountryTable { get; set; }
    ManagementFunction ActiveComponent { get; set; }
    string Filter { get; set; }
    
    Task LoadCountryPage(QueryParams parameters);
    Task CreateCountry();
    Task UpdateCountry();
    Task DeleteCountry(long countryId);
    CountryDto GetNewCountry();
    void SetSelectedCountry(long countryId);
}