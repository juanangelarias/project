using AutoMapper;
using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Model.Sorts;
using CM.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CM.Repositories;

public class ProductComboRepository: BaseRepository<ProductCombo, ProductComboDto>, IProductComboRepository
{
    private readonly IMapper _mapper;

    public ProductComboRepository(IMapper mapper, DbContext db) : base(mapper, db)
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductComboDto>> GetAllAsync()
    {
        var qry = await GetQuery()
            .ToListAsync();

        return _mapper.Map<List<ProductComboDto>>(qry);
    }

    public async Task<IEnumerable<ProductDto>> GetChildren(long productId)
    {
        var qry = await GetQuery()
           .Where(r => r.ParentProductId == productId)
           .Select(s=>s.ChildProduct)
           .ToListAsync();

        return _mapper.Map<List<ProductDto>>(qry);
    }

    public async Task<PagedResponse<ProductComboDto>> GetPageAsync(ProductComboQueryParams parameters)
    {
        var qry = GetQuery()
            .Include(i=>i.ParentProduct)
            .Include(i=>i.ChildProduct)
            .Where(r=>r.ParentProductId == parameters.ParentProductId);

        var sort = parameters.Sort ?? ProductComboSorts.Name;

        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            var filter = parameters.Filter.ToLower();
            qry = qry.Where(
                r => r.ChildProduct!.Name!.ToLower().Contains(filter)
            ) as IIncludableQueryable<ProductCombo, Product?>;
        }

        var queryable = GetPaginatedQueryable(parameters, qry);

        var result = queryable.query != null ? await queryable.query.ToListAsync() : null;

        result = sort switch
        {
            _ => parameters.Descending
                ? result!
                    .OrderByDescending(o => o.ChildProduct!.Name)
                    .ToList()
                : result!
                    .OrderBy(o => o.ChildProduct!.Name)
                    .ToList()
        };

        var mappedResult = _mapper.Map<List<ProductComboDto>>(result);

        var response = new PagedResponse<ProductComboDto>(mappedResult, queryable.rowCount);

        return response;
    }

    public async Task<IEnumerable<ProductComboDto>> Autocomplete(ProductComboAutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null ? string.Empty : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0 ? 5 : (int)parameters.Count;

        var list = await GetQuery()
            .Where(r =>
                r.ParentProductId == parameters.ParentProductId &&
                r.ChildProduct!.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToListAsync();

        return _mapper.Map<List<ProductComboDto>>(list);
    }

    public async Task<IEnumerable<ProductDto>> UpdateChildren(long parentProductId, List<long> idsToBeAdded,
        List<long> idsToBeDeleted)
    {
        var toBeAdded = idsToBeAdded
            .Select(s => new ProductComboDto
            {
                ParentProductId = parentProductId,
                ChildProductId = s
            });

        var toBeDeleted = await GetQuery()
            .Where(r => r.ParentProductId == parentProductId &&
                        idsToBeDeleted.Contains(Convert.ToInt64(r.ChildProductId)))
            .Select(s=>s.Id)
            .ToListAsync();

        await DeleteManyAsync(toBeDeleted);

        await CreateManyAsync(toBeAdded);

        return await GetChildren(parentProductId);
    }
}