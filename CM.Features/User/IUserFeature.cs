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
    Task<PasswordMailData?> GetUserFromToken(string token);
    Task<IEnumerable<RoleDto>> GetUserRoles(long userId);
    Task<IEnumerable<RoleDto>> SetUserRoles(long userId, List<RoleDto> roles);
    Task AddRoleToUser(long userId, long roleId);
    Task RemoveRoleFromUser(long userId, long roleId);
}