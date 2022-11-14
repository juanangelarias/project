using System.Text.Json;
using CM.Common.Configuration.Models;
using CM.Core.Authentication;
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
    private readonly IUserRequestTokenRepository _userRequestTokenRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMailService _mailService;
    private readonly PasswordSettings _passwordSettings;

    public UserFeature(IUserRepository userRepository, IUserPasswordRepository userPasswordRepository,
        IUserRequestTokenRepository userRequestTokenRepository, IEncryptionService encryptionService,
        IJwtGenerator jwtGenerator, IMailService mailService, PasswordSettings passwordSettings)
    {
        _userRepository = userRepository;
        _userPasswordRepository = userPasswordRepository;
        _userRequestTokenRepository = userRequestTokenRepository;
        _encryptionService = encryptionService;
        _jwtGenerator = jwtGenerator;
        _mailService = mailService;
        _passwordSettings = passwordSettings;
    }

    public async Task<LoginResponse?> Login(LoginData login)
    {
        if (login.UserName == null)
            return null;
        
        var user = await _userRepository.GetByUserName(login.UserName);
        if (user == null)
            return null;

        var userPassword = await _userPasswordRepository.GetCurrentPassword(user.Id);
        if (userPassword == null)
            return null;

        var hashedPassword = _encryptionService.Encrypt(login.Password!, userPassword.SecurityStamp!);

        if (hashedPassword == userPassword.PasswordHash)
        {
            return new LoginResponse
            {
                User = await _userRepository.GetByIdExpandedAsync(user.Id),
                Token = await _jwtGenerator.GetToken(user.Id),
            };
        }

        return null;
    }

    public async Task<bool> ChangePassword(ResetPassword data)
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

    public async Task<bool> ResetPassword(ResetPassword data)
    {
        var user = await _userRepository.GetByUserName(data.UserName);
        if (user == null)
            return false;

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

        var token = Guid.NewGuid().ToString();

        var requestToken = new UserRequestTokenDto
        {
            UserId = userId,
            Token = token,
            Emitted = DateTime.Now,
            Expires = DateTime.Now.AddHours(_passwordSettings.WelcomeTokenExpirationInHours)
        };

        await _userRequestTokenRepository.CreateAsync(requestToken);

        var data = new PasswordMailData
        {
            UserId = userId,
            Email = user.Email,
            Expiration = DateTime.UtcNow.AddMinutes(deltaTime),
            FullName = user.FullName,
            Token = token,
            Type = template
        };

        _mailService.SendTemplate(data);
    }

    public async Task<PasswordMailData> GetUserFromToken(string token)
    {
        var tokenDef = await _userRequestTokenRepository.GetTokenDefinition(token);

        if (tokenDef == null)
        {
            return null;
        }

        var data = new PasswordMailData
        {
            Email = tokenDef.User.UserName,
            Expiration = tokenDef.Expires,
            FullName = tokenDef.User.FullName,
            Token = tokenDef.Token,
            Type = EmailTemplate.UserInvitation,
            UserId = tokenDef.User.Id
        };
        return data;
    }
}