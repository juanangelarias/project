using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class UserRoleRepository: BaseRepository<UserRole, UserRoleDto>, IUserRoleRepository
{
    private readonly IMapper _mapper;

    public UserRoleRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetUserRoles(long userId)
    {
        var qry = await GetQuery()
            .Include(i => i.Role)
            .Where(r => r.UserId == userId)
            .OrderBy(o => o.UserId)
            .ThenBy(t => t.Role!.Name)
            .Select(s => s.Role)
            .ToListAsync();

        return _mapper.Map<List<RoleDto>>(qry);
    }
}