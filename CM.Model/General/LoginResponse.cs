using CM.Model.Dto;

namespace CM.Model.General;

public class LoginResponse
{
    public UserDto User { get; set; } = new();
    //public ConferenceDto Conference { get; set; } = new();
    //public List<RoleDto> Roles { get; set; } = new();
    public string Token { get; set; } = "";
}