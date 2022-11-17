using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IRoleRepository: IBaseRepository<Role,RoleDto>
{
    Task<PagedResponse<RoleDto>> GetPageAsync(QueryParams parameters);
}