using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class UserRepository: BaseRepository<User, UserDto>, IUserRepository
{
    private readonly IMapper _mapper;
    
    public UserRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Autocomplete(string filter, byte count)
    {
        filter = filter.ToLower();
        var qry = GetQuery()
            .Where(r => r.FirstName!.ToLower().Contains(filter) ||
                        r.LastName!.ToLower().Contains(filter))
            .OrderBy(o => o.FirstName)
            .ThenBy(t => t.LastName)
            .Take(count);

        var result = await qry.ToListAsync();

        return _mapper.Map<IEnumerable<UserDto>>(result);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(bool expanded)
    {
        return _mapper.Map<List<UserDto>>(await GetQuery().ToListAsync());
    }

    public async Task<PagedResponse<UserDto>> GetAllPagedAsync(QueryParams parameters, bool expanded)
    {
        var query = GetQuery();
        var sort = parameters.Sort ?? UserSorts.UserName;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            query = query.Where(r =>
                (r.FirstName + ' ' + r.LastName).ToLower().Contains(filter) ||
                r.UserName!.ToLower().Contains(filter));
        }

        query = sort switch
        {
            UserSorts.Name => parameters.Descending
                ? query.OrderByDescending(s => s.FirstName)
                    .ThenByDescending(s => s.LastName)
                : query.OrderBy(s => s.FirstName)
                    .ThenBy(s => s.LastName),
            UserSorts.UserName => parameters.Descending
                ? query.OrderByDescending(s => s.UserName)
                : query.OrderBy(s => s.UserName),
            _ => throw new ArgumentException("Invalid sort parameter")
        };

        /*if (parameters.Expand)
        { }*/

        var queryable = GetPaginatedQueryable(parameters, query);

        var result = queryable.query != null
            ? await queryable.query.ToListAsync()
            : null;

        var mappedResult = _mapper.Map<IEnumerable<UserDto>>(result);

        var response = new PagedResponse<UserDto>(mappedResult, queryable.rowCount);

        return response;
    }
    
    public async Task<UserDto> GetByIdExpandedAsync(long id)
    {
        var record = await GetQuery()
            .Include(i => i.UserRoles)!
            .ThenInclude(i=>i.Role)
            .FirstOrDefaultAsync(f => f.Id == id);

        return _mapper.Map<UserDto>(record);
    }

    public async Task<UserDto?> GetByUserName(string username)
    {
        var user = await GetQuery()
            .FirstOrDefaultAsync(x => x.UserName == username);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await GetQuery()
            .FirstOrDefaultAsync(x => x.Email == email);

        return _mapper.Map<UserDto>(user);      
    }

    public async Task<IEnumerable<RoleDto>> GetUserRoles(long userId)
    {
        var user = await GetQuery()
            .Include(i => i.UserRoles)!
            .ThenInclude(t => t.Role)
            .FirstOrDefaultAsync(r => r.Id == userId);

        var roles = user?.UserRoles!
            .Select(s => s.Role)
            .ToList();

        return _mapper.Map<List<RoleDto>>(roles);
    }
}