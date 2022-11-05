using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IUserRepository: IBaseRepository<User, UserDto>
{
    Task<IEnumerable<UserDto>> Autocomplete(string filter, byte count);
    Task<IEnumerable<UserDto>> GetAllAsync(bool expanded);
    Task<PagedResponse<UserDto>> GetAllPagedAsync(QueryParams parameters, bool expanded);
    Task<UserDto> GetByIdExpandedAsync(long id);
    Task<UserDto> GetByUserName(string username);
    Task<UserDto> GetByEmailAsync(string email);
    Task<IEnumerable<RoleDto>> GetUserRoles(long userId);
}