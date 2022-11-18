using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IClubService: IBaseService<ClubDto>
{
    Task<IEnumerable<ClubDto>> Autocomplete(AutoCompleteParams parameters);
}