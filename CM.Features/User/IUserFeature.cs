using CM.Model.Dto;
using CM.Model.Enum;
using CM.Model.General;

namespace CM.Features;

public interface IUserFeature
{
    Task<LoginResponse?> Login(LoginRequest login);
    Task<bool> ResetPassword(ResetPassword data);
    Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest data);
    Task SendMail(long userId, EmailTemplate template);
    Task<PasswordMailData> GetUserFromToken(string token);
}