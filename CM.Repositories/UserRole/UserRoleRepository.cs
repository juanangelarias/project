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

    public async Task<IEnumerable<UserRoleDto>> GetByUser(long userId)
    {
        var qry = await GetQuery()
            .Where(r=>r.UserId == userId)
            .ToListAsync();

        return _mapper.Map<List<UserRoleDto>>(qry);
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

    public async Task RemoveUserRole(long userId, long roleId)
    {
        var record = GetQuery()
            .FirstOrDefault(r => r.UserId == userId &&
                                 r.RoleId == roleId);

        if (record != null)
        {
            await DeleteAsync(record.Id);
        }
    }
}