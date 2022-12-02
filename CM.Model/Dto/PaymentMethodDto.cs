using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class PaymentMethodDto: BaseDto
{
    public string? Name { get; set; }
    public bool? RequiredBankName { get; set; }
    public bool? RequireDocumentNumber { get; set; }
}