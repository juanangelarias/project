using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.State;

public interface IGeneralStateProvider
{
    DateTime? Expires { get; set; }
    string? Token { get; set; }
    UserDto? User { get; set; }
    void Initialize();
    void SetLocalStorage(LoginResponse data);
    void Logout();
}