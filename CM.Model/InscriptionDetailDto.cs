using CM.Model.Dto;
using CM.Model.Dto.Base;

namespace CM.Model;

public class InscriptionDetailDto: BaseDto
{
    public long InscriptionId { get; set; }
    public int Line { get; set; }
    public long? ProductId { get; set; }
    public long? MemberId { get; set; }
    public string? Notes { get; set; }
    public decimal Amount { get; set; }

    public InscriptionDto? Inscription { get; set; }
    public ProductDto? Product { get; set; }
    public MemberDto? Member { get; set; }
}