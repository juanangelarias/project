using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;

namespace CM.Repositories;

public class RoleRepository: BaseRepository<Role,RoleDto>, IRoleRepository
{
    public RoleRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
    }
}