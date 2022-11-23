using CM.Entities;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories.Base;

namespace CM.Repositories;

public interface IProductRepository: IBaseRepository<Product, ProductDto>
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<PagedResponse<ProductDto>> GetPageAsync(QueryParams parameters);
    Task<IEnumerable<ProductDto>> Autocomplete(AutoCompleteParams parameters);
}