using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class ConferenceDto: BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? HostClubId { get; set; }
    public long? PrimaryCurrencyId { get; set; }
    public long? SecondaryCurrencyId { get; set; }
    public decimal? ExchangeRate { get; set; }

    public ClubDto? HostClub { get; set; }
    public CurrencyDto? PrimaryCurrency { get; set; }
    public CurrencyDto? SecondaryCurrency { get; set; }
}