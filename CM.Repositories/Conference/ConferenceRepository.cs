using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class ConferenceRepository: BaseRepository<Conference, ConferenceDto>, IConferenceRepository
{
    private readonly IMapper _mapper;

    public ConferenceRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ConferenceDto>> GetAllAsync()
    {
        var qry = await GetQuery()
            .Include(i=>i.HostClub)
            .Include(i => i.HostClub!.Country)
            .Include(i => i.HostClub!.ClubType)
            .Include(i=>i.PrimaryCurrency)
            .Include(i=>i.SecondaryCurrency)
            .ToListAsync();

        return _mapper.Map<List<ConferenceDto>>(qry);
    }

    public async Task<PagedResponse<ConferenceDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? ConferenceSorts.Name;

        if (parameters.Expand)
        {
            qry = qry.Include(i => i.HostClub)
                .Include(i => i.HostClub!.Country)
                .Include(i => i.HostClub!.ClubType)
                .Include(i => i.PrimaryCurrency)
                .Include(i => i.SecondaryCurrency);
        }
        
        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(
                r => r.Name!.ToLower().Contains(filter)
            );
        }

        qry = sort switch
        {
            ConferenceSorts.Club => parameters.Descending
                ? qry.OrderByDescending(o=>o.HostClub!.Name)
                : qry.OrderBy(o=>o.HostClub!.Name),
            ConferenceSorts.StartDate => parameters.Descending
                ? qry.OrderByDescending(o=>o.StartDate)
                : qry.OrderBy(o=>o.StartDate),
            _
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Name)
                    : qry.OrderBy(o => o.Name)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null ? await queryable.query.ToListAsync() : null;

        var mappedResult = _mapper.Map<List<ConferenceDto>>(result);

        var response = new PagedResponse<ConferenceDto>(mappedResult, queryable.rowCount);

        return response;
    }

    public async Task<IEnumerable<ConferenceDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null ? string.Empty : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0 ? 5 : (int) parameters.Count;

        var list = await GetQuery()
            .Where(r => r.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<ConferenceDto>>(list);
    }
}