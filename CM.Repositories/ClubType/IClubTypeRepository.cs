using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IClubTypeRepository : IBaseRepository<ClubType, ClubTypeDto>
{
    Task<IEnumerable<ClubTypeDto>> GetAllAsync();
    Task<PagedResponse<ClubTypeDto>> GetPageAsync(QueryParams parameters);
    Task<IEnumerable<ClubTypeDto>> Autocomplete(AutoCompleteParams parameters);
}
