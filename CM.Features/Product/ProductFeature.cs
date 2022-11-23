using CM.Model.Dto;
using CM.Repositories;

namespace CM.Features;

public class ProductFeature: IProductFeature
{
    private readonly IProductRepository _productRepository;
    private readonly IProductComboRepository _productComboRepository;

    public ProductFeature(IProductRepository productRepository, IProductComboRepository productComboRepository)
    {
        _productRepository = productRepository;
        _productComboRepository = productComboRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetChildren(long productId)
    {
        return await _productComboRepository.GetChildren(productId);
    }

    public async Task<IEnumerable<ProductDto>> UpdateChildren(long productId, List<ProductDto> children)
    {
        var actualChildren = await _productComboRepository.GetChildren(productId);

        var toBeAdded = children
            .Where(r => actualChildren.All(a => a.Id != r.Id))
            .Select(s=>s.Id)
            .ToList();

        var toBeDeleted = actualChildren
            .Where(r => children.All(a => a.Id != r.Id))
            .Select(s=>s.Id)
            .ToList();

        return await _productComboRepository.UpdateChildren(productId, toBeAdded, toBeDeleted);
    }
}