using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface ICountryRepository: IBaseRepository<Country, CountryDto>
{
    Task<PagedResponse<CountryDto>> GetPageAsync(QueryParams parameters);
    Task<IEnumerable<CountryDto>> Autocomplete(AutoCompleteParams parameters);
}