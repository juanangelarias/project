using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IClubRepository: IBaseRepository<Club, ClubDto>
{
    Task<IEnumerable<ClubDto>> GetAllAsync();
    Task<PagedResponse<ClubDto>> GetPageAsync(QueryParams parameters);
    Task<IEnumerable<ClubDto>> Autocomplete(AutoCompleteParams parameters);
}