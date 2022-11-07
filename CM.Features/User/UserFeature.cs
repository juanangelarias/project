using CM.Core.Services.Encryption;
using CM.Model.Dto;
using CM.Model.General;
using CM.Repositories;

namespace CM.Features;

public class UserFeature : IUserFeature
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPasswordRepository _userPasswordRepository;
    private readonly IEncryptionService _encryptionService;

    public UserFeature(IUserRepository userRepository, IUserPasswordRepository userPasswordRepository,
        IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _userPasswordRepository = userPasswordRepository;
        _encryptionService = encryptionService;
    }

    public async Task<bool> Login(LoginData login)
    {
        var user = await _userRepository.GetByUserName(login.UserName);
        if (user == null)
            return false;

        var userPassword = await _userPasswordRepository.GetCurrentPassword(user.Id);
        if (userPassword == null)
            return false;

        var hashedPassword = _encryptionService.Encrypt(login.Password, userPassword.SecurityStamp);

        return hashedPassword == userPassword.PasswordHash;
    }

    public async Task<bool> ResetPassword(ResetPassword data)
    {
        var user = await _userRepository.GetByUserName(data.UserName);
        if (user == null)
            return false;

        var userPassword = await _userPasswordRepository.GetCurrentPassword(user.Id);
        if (userPassword == null)
            return false;

        var hashedPassword = _encryptionService.OneWayEncrypt(data.Password, userPassword.SecurityStamp);

        if (hashedPassword == userPassword.PasswordHash &&
            !await _userPasswordRepository.PasswordUsedBefore(user.Id, hashedPassword))
        {
            var newSecurityStamp = _encryptionService.CreateSalt();
            var newUserPassword = new UserPasswordDto
            {
                UserId = user.Id,
                Date = DateTime.Now,
                PasswordHash = _encryptionService.OneWayEncrypt(data.NewPassword, newSecurityStamp),
                SecurityStamp = newSecurityStamp
            };

            await _userPasswordRepository.CreateAsync(newUserPassword);

            return true;
        }

        return false;
    }
}