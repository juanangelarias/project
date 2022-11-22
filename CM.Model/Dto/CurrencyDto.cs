using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class CurrencyDto: BaseDto
{
    public string? Name { get; set; }
    public string? Symbol { get; set; }
}