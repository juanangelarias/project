using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IConferenceRepository : IBaseRepository<Conference, ConferenceDto>
{
    Task<IEnumerable<ConferenceDto>> GetAllAsync();
    Task<PagedResponse<ConferenceDto>> GetPageAsync(QueryParams parameters);
    Task<IEnumerable<ConferenceDto>> Autocomplete(AutoCompleteParams parameters);
}
