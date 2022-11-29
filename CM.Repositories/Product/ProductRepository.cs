using AutoMapper;
using CM.Database;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Repositories;

public class ProductRepository : BaseRepository<Product, ProductDto>, IProductRepository
{
    private readonly IMapper _mapper;

    public ProductRepository(IMapper mapper, CmDbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var qry = await GetQuery().ToListAsync();

        return _mapper.Map<List<ProductDto>>(qry);
    }

    public async Task<PagedResponse<ProductDto>> GetPageAsync(QueryParams parameters)
    {
        var qry = GetQuery();
        var sort = parameters.Sort ?? ProductSorts.Name;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(r => r.Name!.ToLower().Contains(filter));
        }

        qry = sort switch
        {
            ProductSorts.Description
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Description)
                    : qry.OrderBy(o => o.Description),
            _
                => parameters.Descending
                    ? qry.OrderByDescending(o => o.Name)
                    : qry.OrderBy(o => o.Name)
        };

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null ? await queryable.query.ToListAsync() : null;

        var mappedResult = _mapper.Map<List<ProductDto>>(result);

        var response = new PagedResponse<ProductDto>(mappedResult, queryable.rowCount);

        return response;
    }

    public async Task<IEnumerable<ProductDto>> Autocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null ? string.Empty : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0 ? 5 : (int)parameters.Count;

        var list = await GetQuery()
            .Where(r => r.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<ProductDto>>(list);
    }
}
