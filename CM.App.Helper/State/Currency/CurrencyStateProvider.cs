using CM.App.Helper.Model.Enum;
using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class CurrencyStateProvider: BaseStateProvider, ICurrencyStateProvider
{
    private readonly ICurrencyService _currencyService;
    
    #region Fields & Properties

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

    #region Selected Currency

    private CurrencyDto? _selectedCurrency;

    public CurrencyDto? SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            _selectedCurrency = value;
            NotifyChanges();
        }
    }

    #endregion

    #region CurrencyTable

    private TableData<CurrencyDto> _currencyTable = new();

    public TableData<CurrencyDto> CurrencyTable
    {
        get => _currencyTable;
        set
        {
            _currencyTable = value;
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

    public CurrencyStateProvider(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task LoadCurrencyPage(QueryParams parameters)
    {
        var page = await _currencyService.GetPage(parameters);
        CurrencyList = page.Items.ToList();
        CurrencyTable = new TableData<CurrencyDto>
        {
            Items = page.Items,
            TotalItems = page.Count
        };
        
        NotifyChanges();
    }

    public async Task CreateCurrency()
    {
        if (SelectedCurrency != null)
        {
            var createdCurrency = await _currencyService.Create(SelectedCurrency);
            if(createdCurrency != null)
                CurrencyList.Add(createdCurrency);

            SelectedCurrency = null;
        }
    }

    public async Task UpdateCurrency()
    {
        if (SelectedCurrency != null)
        {
            var updatedCurrency = await _currencyService.Update(SelectedCurrency);
            if (updatedCurrency != null)
            {
                var currencyToUpdate = CurrencyList.First(f => f.Id == updatedCurrency.Id);
                
                var index = CurrencyList.IndexOf(currencyToUpdate);
                if (index != -1)
                {
                    CurrencyList[index] = updatedCurrency;
                    NotifyChanges();
                }
            }

            SelectedCurrency = null;
        }
    }

    public async Task DeleteCurrency(long currencyId)
    {
        await _currencyService.Delete(currencyId);
        var currencyToDelete = CurrencyList.First(f => f.Id == currencyId);

        CurrencyList.Remove(currencyToDelete);

        SelectedCurrency = null;
        
        NotifyChanges();
    }

    public CurrencyDto GetNewCurrency()
    {
        return new CurrencyDto
        {
            Name = string.Empty,
            Timestamp = Array.Empty<byte>()
        };
    }

    public void SetSelectedCurrency(long currencyId)
    {
        SelectedCurrency = currencyId != 0
            ? CurrencyList.First(f => f.Id == currencyId)
            : GetNewCurrency();
    }
}