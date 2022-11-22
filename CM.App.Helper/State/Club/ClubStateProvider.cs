using CM.App.Helper.Model.Enum;
using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class ClubStateProvider: BaseStateProvider, IClubStateProvider
{
    private readonly IClubService _clubService;
    private readonly ICountryService _countryService;
    private readonly IClubTypeService _clubTypeService;

    #region Field & Properties

    #region Club List

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

    #region Selected Club

    private ClubDto? _selectedClub;

    public ClubDto? SelectedClub
    {
        get => _selectedClub;
        set
        {
            _selectedClub = value;
            NotifyChanges();
        }
    }

    #endregion

    #region ClubTable

    private TableData<ClubDto> _clubTable = new();

    public TableData<ClubDto> ClubTable
    {
        get => _clubTable;
        set
        {
            _clubTable = value;
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

    #region Countries

    private List<CountryDto> _countries = new();

    public List<CountryDto> Countries
    {
        get
        {
            if(!_countries.Any())
                Task.FromResult(GetCountries);
            return _countries;
        }
        set
        {
            _countries = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Club Types

    private List<ClubTypeDto> _clubTypes = new();

    public List<ClubTypeDto> ClubTypes
    {
        get => _clubTypes;
        set
        {
            _clubTypes = value;
            NotifyChanges();
        }
    }

    #endregion

    #endregion

    public ClubStateProvider(IClubService clubService, ICountryService countryService, IClubTypeService clubTypeService)
    {
        _clubService = clubService;
        _countryService = countryService;
        _clubTypeService = clubTypeService;
    }
    
    public async Task LoadClubPage(QueryParams parameters)
    {
        var page = await _clubService.GetPage(parameters);
        ClubList = page.Items.ToList();
        ClubTable = new TableData<ClubDto>
        {
            Items = page.Items,
            TotalItems = page.Count
        };
        
        NotifyChanges();
    }

    public async Task CreateClub()
    {
        if (SelectedClub != null)
        {
            var createdClub = await _clubService.Create(SelectedClub);
            if(createdClub != null)
                ClubList.Add(createdClub);

            SelectedClub = null;
        }
    }

    public async Task UpdateClub()
    {
        if (SelectedClub != null)
        {
            var updatedClub = await _clubService.Update(SelectedClub);
            if (updatedClub != null)
            {
                var clubToUpdate = ClubList.First(f => f.Id == updatedClub.Id);
                
                var index = ClubList.IndexOf(clubToUpdate);
                if (index != -1)
                {
                    ClubList[index] = updatedClub;
                    NotifyChanges();
                }
            }

            SelectedClub = null;
        }
    }

    public async Task DeleteClub(long clubId)
    {
        await _clubService.Delete(clubId);
        var clubToDelete = ClubList.First(f => f.Id == clubId);

        ClubList.Remove(clubToDelete);

        SelectedClub = null;
        
        NotifyChanges();
    }

    public ClubDto GetNewClub()
    {
        return new ClubDto
        {
            Code = string.Empty,
            Name = string.Empty,
            ClubTypeId = 0,
            ClubType = new ClubTypeDto(),
            CountryId = 0,
            Country = new CountryDto(),
            Timestamp = Array.Empty<byte>()
        };
    }

    public void SetSelectedClub(long clubId)
    {
        SelectedClub = clubId != 0
            ? ClubList.First(f => f.Id == clubId)
            : GetNewClub();
    }

    public List<CountryDto> CountryAutocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null
            ? string.Empty
            : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0
            ? 5
            : (int) parameters.Count;

        return Countries
            .Where(r => r.Code!.ToLower().Contains(filter) ||
                        r.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToList();
    }

    public async Task GetCountries()
    {
        _countries = await _countryService.Get();
    }

    public async Task GetClubTypes()
    {
        _clubTypes = await _clubTypeService.Get();
    }
}