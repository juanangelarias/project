using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IUserService: IBaseService<UserDto>
{
    //Task<List<ConferenceDto>?> GetUserConferences(long userId);
    Task<LoginResponse?> Login(LoginRequest login);
    Task<bool?> ResetPassword(ResetPassword data);
    Task<ChangePasswordResponse?> ChangePassword(ChangePasswordRequest changePasswordRequest);
    Task<bool?> ResetForgotPassword(string username, string newPassword);
    Task AddRoleToUser(UserRoleDto userRole);
    Task RemoveRoleFromUser(long userRoleId);
    //Task SetDefaultConference(long userId, ConferenceDto conference);
    Task<bool> SendInvitation(long userId);
    Task<PasswordMailData?> GetUserFromToken(string token);
    Task<IEnumerable<RoleDto>?> GetUserRoles(long userId);
    Task<IEnumerable<RoleDto>?> SetUserRoles(long userId, IEnumerable<RoleDto> roles);
    Task AddRoleToUser(long userId, long roleId);
    Task RemoveRoleFromUser(long userId, long roleId);
}