using CM.App.Helper.Model.Enum;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface IMemberStateProvider
{
    List<MemberDto> MemberList { get; set; }
    MemberDto? SelectedMember { get; set; }
    TableData<MemberDto> MemberTable { get; set; }
    ManagementFunction ActiveComponent { get; set; }
    string Filter { get; set; }
    List<ClubDto> Clubs { get; set; }
    
    Task LoadMemberPage(QueryParams parameters);
    Task CreateMember();
    Task UpdateMember();
    Task DeleteMember(long memberId);
    MemberDto GetNewMember();
    void SetSelectedMember(long memberId);
    List<ClubDto> ClubAutocomplete(AutoCompleteParams parameters);
    Task GetClubs();
}