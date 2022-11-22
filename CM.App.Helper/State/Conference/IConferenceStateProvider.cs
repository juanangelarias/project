using CM.App.Helper.Model.Enum;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface IConferenceStateProvider
{
    List<ConferenceDto> ConferenceList { get; set; }
    ConferenceDto? SelectedConference { get; set; }
    TableData<ConferenceDto> ConferenceTable { get; set; }
    ManagementFunction ActiveComponent { get; set; }
    string Filter { get; set; }

    Task LoadConferencePage(QueryParams parameters);
    Task CreateConference();
    Task UpdateConference();
    Task DeleteConference(long conferenceId);
    ConferenceDto GetNewConference();
    void SetSelectedConference(long conferenceId);
}