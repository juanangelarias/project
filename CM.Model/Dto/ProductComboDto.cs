using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class ProductComboDto : BaseDto
{
    public long? ParentProductId { get; set; }
    public long? ChildProductId { get; set; }

    public ProductDto? ParentProduct { get; set; }
    public ProductDto? ChildProduct { get; set; }
}
