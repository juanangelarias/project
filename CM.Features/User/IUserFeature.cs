using CM.Model.General;

namespace CM.Features;

public interface IUserFeature
{
    Task<bool> Login(LoginData login);
    Task<bool> ResetPassword(ResetPassword data);
}