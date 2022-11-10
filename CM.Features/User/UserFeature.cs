using System.Diagnostics;
using System.Text.Json;
using CM.Common.Configuration.Models;
using CM.Core.Services.Encryption;
using CM.Core.Services.Mail;
using CM.Model.Dto;
using CM.Model.Enum;
using CM.Model.General;
using CM.Repositories;

namespace CM.Features;

public class UserFeature : IUserFeature
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPasswordRepository _userPasswordRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IMailService _mailService;
    private readonly PasswordSettings _passwordSettings;

    public UserFeature(IUserRepository userRepository, IUserPasswordRepository userPasswordRepository,
        IEncryptionService encryptionService, IMailService mailService, PasswordSettings passwordSettings)
    {
        _userRepository = userRepository;
        _userPasswordRepository = userPasswordRepository;
        _encryptionService = encryptionService;
        _mailService = mailService;
        _passwordSettings = passwordSettings;
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

    public async Task SendMail(long userId, EmailTemplate template)
    {
        var user = await _userRepository.GetByIdExpandedAsync(userId);

        var deltaTime = template switch
        {
            EmailTemplate.UserInvitation => _passwordSettings.WelcomeTokenExpirationInHours * 60,
            EmailTemplate.PasswordChanged => _passwordSettings.ResetPasswordTokenExpirationInMinutes,
            EmailTemplate.UserResetPassword => _passwordSettings.ResetPasswordTokenExpirationInMinutes,
            _ => 10
        };

        var data = new PasswordMailData
        {
            UserId = userId,
            Email = user.Email,
            Expiration = DateTime.UtcNow.AddMinutes(deltaTime),
            FullName = user.FullName,
            Token = Guid.NewGuid().ToString(),
            Type = template
        };

        _mailService.SendTemplate(data);
    }

    public PasswordMailData GetUserFromToken(string token)
    {
        var json = _encryptionService.Decrypt(token);
        var data = JsonSerializer.Deserialize<PasswordMailData>(json);

        return data;
    }
}