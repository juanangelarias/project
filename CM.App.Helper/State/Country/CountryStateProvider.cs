using CM.App.Helper.Model.Enum;
using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class CountryStateProvider: BaseStateProvider, ICountryStateProvider
{
    private readonly ICountryService _countryService;

    #region Field & Properties

    #region Country List

    private List<CountryDto> _countryList = new();

    public List<CountryDto> CountryList
    {
        get => _countryList;
        set
        {
            _countryList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Selected Country

    private CountryDto? _selectedCountry;

    public CountryDto? SelectedCountry
    {
        get => _selectedCountry;
        set
        {
            _selectedCountry = value;
            NotifyChanges();
        }
    }

    #endregion

    #region CountryTable

    private TableData<CountryDto> _countryTable = new();

    public TableData<CountryDto> CountryTable
    {
        get => _countryTable;
        set
        {
            _countryTable = value;
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

    #endregion

    public CountryStateProvider(ICountryService countryService)
    {
        _countryService = countryService;
    }
    
    public async Task LoadCountryPage(QueryParams parameters)
    {
        var page = await _countryService.GetPage(parameters);
        CountryList = page.Items.ToList();
        CountryTable = new TableData<CountryDto>
        {
            Items = page.Items,
            TotalItems = page.Count
        };
        
        NotifyChanges();
    }

    public async Task CreateCountry()
    {
        if (SelectedCountry != null)
        {
            var createdCountry = await _countryService.Create(SelectedCountry);
            if(createdCountry != null)
                CountryList.Add(createdCountry);

            SelectedCountry = null;
        }
    }

    public async Task UpdateCountry()
    {
        if (SelectedCountry != null)
        {
            var updatedCountry = await _countryService.Update(SelectedCountry);
            if (updatedCountry != null)
            {
                var countryToUpdate = CountryList.First(f => f.Id == updatedCountry.Id);
                
                var index = CountryList.IndexOf(countryToUpdate);
                if (index != -1)
                {
                    CountryList[index] = updatedCountry;
                    NotifyChanges();
                }
            }

            SelectedCountry = null;
        }
    }

    public async Task DeleteCountry(long countryId)
    {
        await _countryService.Delete(countryId);
        var countryToDelete = CountryList.First(f => f.Id == countryId);

        CountryList.Remove(countryToDelete);

        SelectedCountry = null;
        
        NotifyChanges();
    }

    public CountryDto GetNewCountry()
    {
        return new CountryDto
        {
            Code = string.Empty,
            Name = string.Empty,
            Timestamp = Array.Empty<byte>()
        };
    }

    public void SetSelectedCountry(long countryId)
    {
        SelectedCountry = countryId != 0
            ? CountryList.First(f => f.Id == countryId)
            : GetNewCountry();
    }
}