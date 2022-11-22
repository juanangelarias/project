using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IConferenceService : IBaseService<ConferenceDto>
{
    Task<IEnumerable<ConferenceDto>> Autocomplete(AutoCompleteParams parameters);
}
