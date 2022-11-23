using CM.Model.Dto;
using CM.Model.General;

namespace CM.Repositories;

public interface IProductComboRepository
{
    Task<IEnumerable<ProductComboDto>> GetAllAsync();
    Task<IEnumerable<ProductDto>> GetChildren(long productId);
    Task<PagedResponse<ProductComboDto>> GetPageAsync(ProductComboQueryParams parameters);
    Task<IEnumerable<ProductComboDto>> Autocomplete(ProductComboAutoCompleteParams parameters);

    Task<IEnumerable<ProductDto>> UpdateChildren(long parentProductId, List<long> idsToBeAdded,
        List<long> idsToBeDeleted);
}
