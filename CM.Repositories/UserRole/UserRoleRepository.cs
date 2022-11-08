using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public class UserRoleRepository: BaseRepository<UserRole, UserRoleDto>, IUserRoleRepository
{
    public UserRoleRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        
    }
}