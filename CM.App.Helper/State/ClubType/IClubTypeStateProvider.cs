using CM.App.Helper.Model.Enum;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface IClubTypeStateProvider
{
    List<ClubTypeDto> ClubTypeList { get; set; }
    ClubTypeDto? SelectedClubType { get; set; }
    TableData<ClubTypeDto> ClubTypeTable { get; set; }
    ManagementFunction ActiveComponent { get; set; }
    string Filter { get; set; }

    Task LoadClubTypePage(QueryParams parameters);
    Task CreateClubType();
    Task UpdateClubType();
    Task DeleteClubType(long clubTypeId);
    ClubTypeDto GetNewClubType();
    void SetSelectedClubType(long clubTypeId);
}