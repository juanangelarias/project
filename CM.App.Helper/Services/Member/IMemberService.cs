using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IMemberService: IBaseService<MemberDto>
{
    Task<IEnumerable<MemberDto>> Autocomplete(AutoCompleteParams parameters);
}