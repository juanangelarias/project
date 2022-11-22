using CM.App.Helper.Model.Enum;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface IClubStateProvider
{
    List<ClubDto> ClubList { get; set; }
    ClubDto? SelectedClub { get; set; }
    TableData<ClubDto> ClubTable { get; set; }
    ManagementFunction ActiveComponent { get; set; }
    string Filter { get; set; }
    List<CountryDto> Countries { get; set; }
    List<ClubTypeDto> ClubTypes { get; set; }
    
    Task LoadClubPage(QueryParams parameters);
    Task CreateClub();
    Task UpdateClub();
    Task DeleteClub(long clubId);
    ClubDto GetNewClub();
    void SetSelectedClub(long clubId);
    List<CountryDto> CountryAutocomplete(AutoCompleteParams parameters);
    Task GetCountries();
    Task GetClubTypes();
}