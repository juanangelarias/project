using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class UserPasswordDto: BaseDto
{
    public long UserId { get; set; }
    public DateTime Date { get; set; }
    public string SecurityStamp  { get; set; }
    public string PasswordHash {get;set;}

    public UserDto User { get; set; }
}