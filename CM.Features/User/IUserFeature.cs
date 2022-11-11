using CM.Model.Dto;
using CM.Model.Enum;
using CM.Model.General;

namespace CM.Features;

public interface IUserFeature
{
    Task<bool> Login(LoginData login);
    Task<bool> ChangePassword(ResetPassword data);
    Task<bool> ResetPassword(ResetPassword data);
    Task SendMail(long userId, EmailTemplate template);
    Task<PasswordMailData> GetUserFromToken(string token);
}