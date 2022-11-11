using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class UserRequestTokenDto: BaseDto
{
    public long? UserId { get; set; }
    public string? Token { get; set; }
    public DateTime? Emitted { get; set; }
    public DateTime? Expires { get; set; }

    public UserDto? User { get; set; }
}