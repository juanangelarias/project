using CM.App.Helper.Model.Enum;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface ICurrencyStateProvider
{
    List<CurrencyDto> CurrencyList { get; set; }
    CurrencyDto? SelectedCurrency { get; set; }
    TableData<CurrencyDto> CurrencyTable { get; set; }
    ManagementFunction ActiveComponent { get; set; }
    string Filter { get; set; }

    Task LoadCurrencyPage(QueryParams parameters);
    Task CreateCurrency();
    Task UpdateCurrency();
    Task DeleteCurrency(long currencyId);
    CurrencyDto GetNewCurrency();
    void SetSelectedCurrency(long currencyId);
}