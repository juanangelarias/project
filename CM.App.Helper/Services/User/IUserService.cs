using CM.Model.Dto;
using CM.Model.General;

namespace CM.App.Helper.Services;

public interface IUserService: IBaseService<UserDto>
{
    //Task<List<ConferenceDto>?> GetUserConferences(long userId);
    Task<LoginResponse?> Login(LoginData login);
    Task<bool?> ResetPassword(string username, string oldPassword, string newPassword);
    Task<bool?> ResetForgotPassword(string username, string newPassword);
    Task AddRoleToUser(UserRoleDto userRole);
    Task RemoveRoleFromUser(long userRoleId);
    //Task SetDefaultConference(long userId, ConferenceDto conference);
    Task<bool> SendInvitation(long userId);
}