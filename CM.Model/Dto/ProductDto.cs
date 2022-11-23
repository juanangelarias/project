using CM.Model.Dto.Base;
using CM.Model.Enum;

namespace CM.Model.Dto;

public class ProductDto : BaseDto
{
    public long? ConferenceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? PrimaryPrice { get; set; }
    public decimal? SecondaryPrice { get; set; }
    public bool? RequireName { get; set; }
    public bool? PrintBadge { get; set; }
    public ProductType ProductType { get; set; }

    public ConferenceDto? Conference { get; set; }
}
