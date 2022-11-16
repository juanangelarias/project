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
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IMailService _mailService;
    private readonly PasswordSettings _passwordSettings;

    public UserFeature(IUserRepository userRepository, IUserPasswordRepository userPasswordRepository,
        IUserRequestTokenRepository userRequestTokenRepository, IUserRoleRepository userRoleRepository,
        IEncryptionService encryptionService, IJwtGenerator jwtGenerator, IMailService mailService, 
        PasswordSettings passwordSettings)
    {
        _userRepository = userRepository;
        _userPasswordRepository = userPasswordRepository;
        _userRequestTokenRepository = userRequestTokenRepository;
        _userRoleRepository = userRoleRepository;
        _encryptionService = encryptionService;
        _jwtGenerator = jwtGenerator;
        _mailService = mailService;
        _passwordSettings = passwordSettings;
    }

    public async Task<LoginResponse?> Login(LoginRequest login)
    {
        if (login.Username == null)
            return null;
        
        var user = await _userRepository.GetByUserName(login.Username);
        if (user == null)
            return null;

        var userPassword = await _userPasswordRepository.GetCurrentPassword(user.Id);
        if (userPassword == null)
            return null;

        var hashedPassword = _encryptionService.OneWayEncrypt(login.Password!, userPassword.SecurityStamp!);

        if (hashedPassword == userPassword.PasswordHash)
        {
            user = await _userRepository.GetByIdExpandedAsync(user.Id);
            user.Roles = new List<UserRoleDto>();
            var roles = await _userRoleRepository.GetUserRoles(user.Id);
            foreach (var role in roles)
            {
                user.Roles.Add(new UserRoleDto
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    Role = role
                });
            }
            
            var token = await _jwtGenerator.GetToken(user.Id);
            user.Expires = token.Expires;
            user.Token = token.Value;
                
            return new LoginResponse
            {
                User = user,
                Token = token.Value
            };
        }

        return null;
    }

    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest data)
    {
        var user = await _userRepository.GetByIdExpandedAsync(data.UserId);

        var userPassword = await _userPasswordRepository.GetCurrentPassword(user.Id);
        if (userPassword == null)
            return new ChangePasswordResponse
            {
                Success = false,
                Message = "User does not have password."
            };

        var hashedPassword = _encryptionService.OneWayEncrypt(data.OldPassword, userPassword.SecurityStamp!);

        if (hashedPassword != userPassword.PasswordHash ||
            await _userPasswordRepository.PasswordUsedBefore(user.Id, hashedPassword))
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Message = "Old password does not match."
            };
        }

        var newSecurityStamp = _encryptionService.CreateSalt();
        var newUserPassword = new UserPasswordDto
        {
            UserId = user.Id,
            Date = DateTime.Now,
            PasswordHash = _encryptionService.OneWayEncrypt(data.NewPassword, newSecurityStamp),
            SecurityStamp = newSecurityStamp
        };

        await _userPasswordRepository.CreateAsync(newUserPassword);

        return new ChangePasswordResponse
        {
            Success = true,
            Message = ""
        };

    }

    public async Task<bool> ResetPassword(ResetPassword data)
    {
        var user = await _userRepository.GetByUserName(data.Username);
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