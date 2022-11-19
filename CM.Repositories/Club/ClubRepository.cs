using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class ClubRepository: BaseRepository<Club,ClubDto>, IClubRepository
{
    private readonly IMapper _mapper;

    public ClubRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<PagedResponse<ClubDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? ClubSorts.Name;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(r =>
                r.Code!.ToLower().Contains(filter) ||
                r.Name!.ToLower().Contains(filter));
        }

        qry = sort switch
        {
            ClubSorts.Code => parameters.Descending
                ? qry.OrderByDescending(o => o.Code)
                : qry.OrderBy(o => o.Code),
            _ => parameters.Descending
                ? qry.OrderByDescending(o => o.Name)
                : qry.OrderBy(o => o.Name)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null
            ? await queryable.query.ToListAsync()
            : null;

        var mappedResult = _mapper.Map<List<ClubDto>>(result);

        var response = new PagedResponse<ClubDto>(mappedResult, queryable.rowCount);

        return response;
    }

    public async Task<IEnumerable<ClubDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null
            ? string.Empty
            : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0
            ? 5
            : (int) parameters.Count;

        var list = await GetQuery()
            .Where(r => r.Code!.ToLower().Contains(filter) ||
                        r.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<ClubDto>>(list);
    }
}