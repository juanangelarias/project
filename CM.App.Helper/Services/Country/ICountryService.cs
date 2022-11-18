using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface ICountryService: IBaseService<CountryDto>
{
    Task<IEnumerable<CountryDto>> Autocomplete(AutoCompleteParams parameters);
}