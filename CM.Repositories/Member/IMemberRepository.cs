using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IMemberRepository: IBaseRepository<Member, MemberDto>
{
    Task<IEnumerable<MemberDto>> GetAllAsync();
    Task<PagedResponse<MemberDto>> GetPageAsync(QueryParams parameters);
    Task<IEnumerable<MemberDto>> Autocomplete(AutoCompleteParams parameters);
}