using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class MemberDto: BaseDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Alias { get; set; }
    public long? ClubId { get; set; }
    public bool IsActive { get; set; }

    public ClubDto? Club { get; set; }
}