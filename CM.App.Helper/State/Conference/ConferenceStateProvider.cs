using CM.App.Helper.Model.Enum;
using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class ConferenceStateProvider: BaseStateProvider, IConferenceStateProvider
{
    private readonly IConferenceService _conferenceService;
    private readonly IClubService _clubService;
    private readonly ICurrencyService _currencyService;

    #region Fields & Properties

    #region Conference List

    private List<ConferenceDto> _conferenceList = new();

    public List<ConferenceDto> ConferenceList
    {
        get => _conferenceList;
        set
        {
            _conferenceList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Selected Conference

    private ConferenceDto? _selectedConference;

    public ConferenceDto? SelectedConference
    {
        get => _selectedConference;
        set
        {
            _selectedConference = value;
            NotifyChanges();
        }
    }

    #endregion

    #region ConferenceTable

    private TableData<ConferenceDto> _conferenceTable = new();

    public TableData<ConferenceDto> ConferenceTable
    {
        get => _conferenceTable;
        set
        {
            _conferenceTable = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Active Component

    private ManagementFunction _activeComponent = ManagementFunction.List;

    public ManagementFunction ActiveComponent
    {
        get => _activeComponent;
        set
        {
            _activeComponent = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Filter

    private string _filter = string.Empty;

    public string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            NotifyChanges();
        }
    }

    #endregion
    
    #region Currency List

    private List<CurrencyDto> _currencyList = new();

    public List<CurrencyDto> CurrencyList
    {
        get => _currencyList;
        set
        {
            _currencyList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region ClubList

    private List<ClubDto> _clubList = new();

    public List<ClubDto> ClubList
    {
        get => _clubList;
        set
        {
            _clubList = value;
            NotifyChanges();
        }
    }

    #endregion
    
    #endregion

    public ConferenceStateProvider(IConferenceService conferenceService, IClubService clubService, ICurrencyService currencyService)
    {
        _conferenceService = conferenceService;
        _clubService = clubService;
        _currencyService = currencyService;
    }

    public async Task LoadConferencePage(QueryParams parameters)
    {
        var page = await _conferenceService.GetPage(parameters);
        ConferenceList = page.Items.ToList();
        ConferenceTable = new TableData<ConferenceDto>
        {
            Items = page.Items,
            TotalItems = page.Count
        };
        
        NotifyChanges();
    }

    public async Task CreateConference()
    {
        if (SelectedConference != null)
        {
            var createdConference = await _conferenceService.Create(SelectedConference);
            if(createdConference != null)
                ConferenceList.Add(createdConference);

            SelectedConference = null;
        }
    }

    public async Task UpdateConference()
    {
        if (SelectedConference != null)
        {
            var updatedConference = await _conferenceService.Update(SelectedConference);
            if (updatedConference != null)
            {
                var conferenceToUpdate = ConferenceList.First(f => f.Id == updatedConference.Id);
                
                var index = ConferenceList.IndexOf(conferenceToUpdate);
                if (index != -1)
                {
                    ConferenceList[index] = updatedConference;
                    NotifyChanges();
                }
            }

            SelectedConference = null;
        }
    }

    public async Task DeleteConference(long conferenceId)
    {
        await _conferenceService.Delete(conferenceId);
        var conferenceToDelete = ConferenceList.First(f => f.Id == conferenceId);

        ConferenceList.Remove(conferenceToDelete);

        SelectedConference = null;
        
        NotifyChanges();
    }

    public ConferenceDto GetNewConference()
    {
        return new ConferenceDto
        {
            Name = string.Empty,
            Timestamp = Array.Empty<byte>()
        };
    }

    public async Task SetSelectedConference(long conferenceId)
    {
        await LoadClubs();
        await LoadCurrencies();
        
        SelectedConference = conferenceId != 0
            ? ConferenceList.First(f => f.Id == conferenceId)
            : GetNewConference();
    }

    private async Task LoadCurrencies()
    {
        if (!CurrencyList.Any())
        {
            CurrencyList = await _currencyService.Get();
        }
    }

    private async Task LoadClubs()
    {
        if (!ClubList.Any())
        {
            ClubList = await _clubService.Get();
        }
    }
}