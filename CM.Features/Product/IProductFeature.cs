using CM.Model.Dto;

namespace CM.Features;

public interface IProductFeature
{
    Task<IEnumerable<ProductDto>> GetChildren(long productId);
    Task<IEnumerable<ProductDto>> UpdateChildren(long productId, List<ProductDto> children);
}