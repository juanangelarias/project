using CM.Model.Dto.Base;

namespace CM.Model.Dto;

public class UserRoleDto: BaseDto
{
    public long UserId { get; set; }
    public long RoleId { get; set; }

    public UserDto User { get; set; }
    public RoleDto Role { get; set; }
}