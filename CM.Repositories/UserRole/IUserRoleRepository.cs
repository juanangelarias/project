using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IUserRoleRepository: IBaseRepository<UserRole, UserRoleDto>
{
    Task<IEnumerable<UserRoleDto>> GetByUser(long userId);
    Task<IEnumerable<RoleDto>> GetUserRoles(long userId);
    Task RemoveUserRole(long userId, long roleId);
}