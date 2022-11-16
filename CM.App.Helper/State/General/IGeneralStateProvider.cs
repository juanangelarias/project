using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.State;

public interface IGeneralStateProvider
{
    DateTime? Expires { get; set; }
    string? Token { get; set; }
    long? UserId { get; set; }
    string? Username { get; set; }
    string? UserFullName { get; set; }
    List<string>? Roles { get; set; }
    // Conference
    void Initialize();
    void SetLocalStorage(LoginResponse data);
    void Logout();
}