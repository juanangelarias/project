using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class MemberRepository : BaseRepository<Member, MemberDto>, IMemberRepository
{
    private readonly IMapper _mapper;

    public MemberRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberDto>> GetAllAsync()
    {
        var qry = await GetQuery()
            .Include(i => i.Club)
            .Include(i => i.Club!.Country)
            .Include(i => i.Club!.ClubType)
            .ToListAsync();

        return _mapper.Map<List<MemberDto>>(qry);
    }

    public async Task<PagedResponse<MemberDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? ClubSorts.Name;

        if (parameters.Expand)
        {
            qry = qry.Include(i => i.Club)
                .Include(i => i.Club!.Country)
                .Include(i => i.Club!.ClubType);
        }

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(
                r => r.Code!.ToLower().Contains(filter) || r.Name!.ToLower().Contains(filter)
            );
        }

        qry = sort switch
        {
            MemberSorts.Code
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Code)
                    : qry.OrderBy(o => o.Code),
            MemberSorts.Alias
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Alias)
                    : qry.OrderBy(o => o.Alias),
            MemberSorts.Club
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Club!.Name)
                    : qry.OrderBy(o => o.Club!.Name),
            _
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Name)
                    : qry.OrderBy(o => o.Name)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null ? await queryable.query.ToListAsync() : null;

        var mappedResult = _mapper.Map<List<MemberDto>>(result);

        var response = new PagedResponse<MemberDto>(mappedResult, queryable.rowCount);

        return response;
    }

    public async Task<IEnumerable<MemberDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null ? string.Empty : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0 ? 5 : (int)parameters.Count;

        var list = await GetQuery()
            .Where(
                r =>
                    r.Code!.ToLower().Contains(filter)
                    || r.Name!.ToLower().Contains(filter)
                    || r.Alias!.ToLower().Contains(filter)
            )
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<MemberDto>>(list);
    }
}
