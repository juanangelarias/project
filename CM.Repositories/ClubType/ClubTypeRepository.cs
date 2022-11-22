using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class ClubTypeRepository: BaseRepository<ClubType, ClubTypeDto>, IClubTypeRepository
{
    private readonly IMapper _mapper;

    public ClubTypeRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClubTypeDto>> GetAllAsync()
    {
        var qry = await GetQuery()
            .ToListAsync();

        return _mapper.Map<List<ClubTypeDto>>(qry);
    }

    public async Task<PagedResponse<ClubTypeDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? ClubTypeSorts.Name;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(
                r => r.Name!.ToLower().Contains(filter)
            );
        }

        qry = sort switch
        {
            _
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Name)
                    : qry.OrderBy(o => o.Name)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null ? await queryable.query.ToListAsync() : null;

        var mappedResult = _mapper.Map<List<ClubTypeDto>>(result);

        var response = new PagedResponse<ClubTypeDto>(mappedResult, queryable.rowCount);

        return response;
    }

    public async Task<IEnumerable<ClubTypeDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null ? string.Empty : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0 ? 5 : (int)parameters.Count;

        var list = await GetQuery()
            .Where(r => r.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<ClubTypeDto>>(list);
    }
}