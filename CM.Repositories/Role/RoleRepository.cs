using AutoMapper;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class RoleRepository: BaseRepository<Role,RoleDto>, IRoleRepository
{
    public RoleRepository(IMapper mapper, DbContext db) : base(mapper, db)
    {
    }
}