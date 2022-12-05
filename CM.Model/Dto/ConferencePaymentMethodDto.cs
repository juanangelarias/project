using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class ConferencePaymentMethodDto: BaseDto
{
    public long? ConferenceId { get; set; }
    public long? PaymentMethodId { get; set; }
    public bool? IsAvailable { get; set; }

    public ConferenceDto? Conference { get; set; }
    public PaymentMethodDto? PaymentMethod { get; set; }
}