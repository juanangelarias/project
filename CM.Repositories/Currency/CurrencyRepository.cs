using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class CurrencyRepository: BaseRepository<Currency, CurrencyDto>, ICurrencyRepository
{
    private readonly IMapper _mapper;

    public CurrencyRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<CurrencyDto>> GetAllAsync()
    {
        var qry = await GetQuery()
            .ToListAsync();

        return _mapper.Map<List<CurrencyDto>>(qry);
    }

    public async Task<PagedResponse<CurrencyDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? CurrencySorts.Name;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(
                r => r.Name!.ToLower().Contains(filter)
            );
        }

        qry = sort switch
        {
            CurrencySorts.Symbol => parameters.Descending
            ? qry.OrderByDescending(o=>o.Symbol)
            : qry.OrderBy(o=>o.Symbol),
            _
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Name)
                    : qry.OrderBy(o => o.Name)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null ? await queryable.query.ToListAsync() : null;

        var mappedResult = _mapper.Map<List<CurrencyDto>>(result);

        var response = new PagedResponse<CurrencyDto>(mappedResult, queryable.rowCount);

        return response;
    }

    public async Task<IEnumerable<CurrencyDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null ? string.Empty : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0 ? 5 : (int) parameters.Count;

        var list = await GetQuery()
            .Where(r => r.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<CurrencyDto>>(list);
    }
}