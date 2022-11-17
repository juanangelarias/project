using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class CountryRepository: BaseRepository<Country, CountryDto>, ICountryRepository
{
    private readonly IMapper _mapper;

    public CountryRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<PagedResponse<CountryDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? CountrySorts.Name;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(r =>
                r.Code!.ToLower().Contains(filter) ||
                r.Name!.ToLower().Contains(filter));
        }

        qry = sort switch
        {
            CountrySorts.Code => parameters.Descending
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

        var mappedResult = _mapper.Map<List<CountryDto>>(result);

        var response = new PagedResponse<CountryDto>(mappedResult, queryable.rowCount);

        return response;
    }
}