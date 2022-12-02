using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class InscriptionDto: BaseDto
{
    public long? ConferenceId { get; set; }

    public DateTime? Date { get; set; }
    public long? CurrencyId { get; set; }
    public long? PaymentMethodId { get; set; }
    public string? BankName { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Name { get; set; }

    public string? Notes { get; set; }

    public decimal? GrandTotal { get; set; }
    
    public ConferenceDto? Conference { get; set; }
    public CurrencyDto? Currency { get; set; }
    public PaymentMethodDto? PaymentMethod { get; set; }
    
    public List<InscriptionDetailDto> InscriptionDetails { get; set; }
}