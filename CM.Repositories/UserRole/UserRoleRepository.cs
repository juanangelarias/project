using AutoMapper;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class UserRoleRepository: BaseRepository<UserRole, UserRoleDto>, IUserRoleRepository
{
    public UserRoleRepository(IMapper mapper, DbContext db) : base(mapper, db)
    {
        
    }
}