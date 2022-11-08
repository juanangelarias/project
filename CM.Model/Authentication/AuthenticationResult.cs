using CM.Model.Dto;

namespace CM.Model.Authentication;

public class AuthenticationResult
{
    public string? Message { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public bool Success { get; set; }
    public UserDto? User { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}