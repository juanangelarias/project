using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface ICurrencyRepository : IBaseRepository<Currency, CurrencyDto>
{
    Task<IEnumerable<CurrencyDto>> GetAllAsync();
    Task<PagedResponse<CurrencyDto>> GetPageAsync(QueryParams parameters);
    Task<IEnumerable<CurrencyDto>> Autocomplete(AutoCompleteParams parameters);
}
