using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IClubTypeService: IBaseService<ClubTypeDto>
{
    Task<IEnumerable<ClubTypeDto>> Autocomplete(AutoCompleteParams parameters);
}