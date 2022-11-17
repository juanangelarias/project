using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class RoleRepository: BaseRepository<Role,RoleDto>, IRoleRepository
{
    private readonly IMapper _mapper;

    public RoleRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<PagedResponse<RoleDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? RoleSorts.Code;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(r =>
                r.Code!.ToLower().Contains(filter) ||
                r.Name!.ToLower().Contains(filter) ||
                r.Description!.ToLower().Contains(filter));
        }

        qry = sort switch
        {
            RoleSorts.Code => parameters.Descending
                ? qry.OrderByDescending(o => o.Code)
                : qry.OrderBy(o => o.Code),
            RoleSorts.Name => parameters.Descending
                ? qry.OrderByDescending(o => o.Name)
                : qry.OrderBy(o => o.Name),
            RoleSorts.Description => parameters.Descending
                ? qry.OrderByDescending(o => o.Description)
                : qry.OrderBy(o => o.Description),
            _ => parameters.Descending
                ? qry.OrderByDescending(o => o.Code)
                : qry.OrderBy(o => o.Code)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null
            ? await queryable.query.ToListAsync()
            : null;

        var mappedResult = _mapper.Map<IEnumerable<RoleDto>>(result);

        var response = new PagedResponse<RoleDto>(mappedResult, queryable.rowCount);

        return response;
    }
}