using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface ICurrencyService : IBaseService<CurrencyDto>
{
    Task<IEnumerable<CurrencyDto>> Autocomplete(AutoCompleteParams parameters);
}
