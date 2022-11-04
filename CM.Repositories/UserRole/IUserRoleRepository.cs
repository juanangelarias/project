using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IUserRoleRepository: IBaseRepository<UserRole, UserRoleDto>
{
}