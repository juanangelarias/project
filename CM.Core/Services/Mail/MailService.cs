using System.Text.Json;
using CM.Common.Configuration.Models;
using CM.Core.Services.Encryption;
using CM.Model.Enum;
using CM.Model.General;
using MailKit;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CM.Core.Services.Mail;

public class MailService: IMailService
{
    private readonly MailParameter _parameters;
    private readonly FrontEndParameters _frontEnd;
    private readonly IEncryptionService _encryptionService;
    private bool _success;

    public MailService(MailParameter parameters, FrontEndParameters frontEnd, IEncryptionService encryptionService)
    {
        _parameters = parameters;
        _frontEnd = frontEnd;
        _encryptionService = encryptionService;
    }

    public bool SendEmail(EmailMessage emailMessage)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_parameters.From));
        email.To.AddRange(emailMessage.To.Select(MailboxAddress.Parse));
        email.Subject = emailMessage.Subject;
        email.Body = emailMessage.IsHtml 
            ? new TextPart(TextFormat.Html) {Text = emailMessage.Body} 
            : new TextPart(TextFormat.Plain) {Text = emailMessage.Body};

        return Send(email);
    }

    public bool SendTemplate(PasswordMailData data)
    {
        var json = JsonSerializer.Serialize(new PasswordMailToken
        {
            UserId = data.UserId,
            Expiration = data.Expiration,
            Token = data.Token
        });
        
        var encryptedText = _encryptionService.Encrypt(json);
        
        var body = GetTemplate(data.Type, data.Email, data.FullName, encryptedText);
        var subject = GetSubject(data.Type);

        var email = new MimeMessage(); 
        email.From.Add(new MailboxAddress(_parameters.DisplayName, _parameters.From));
        email.To.Add(new MailboxAddress(data.FullName, data.Email));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) {Text = body};
        
        return Send(email);
    }
    
    private bool Send(MimeMessage email)
    {
        _success = false;

        if (_parameters.SmtpServer == null || _parameters.Port == null)
            return false;
        
        var secureSocketOptions = _parameters.EnableSsl ?? false
            ? SecureSocketOptions.StartTls
            : SecureSocketOptions.None;
        
        using var smtp = new SmtpClient();
        smtp.MessageSent += MessageSent;
        smtp.Connect(_parameters.SmtpServer, _parameters.Port ?? 0, secureSocketOptions);
        smtp.Authenticate(_parameters.Username, _parameters.Password);
        smtp.Send(email);
        smtp.Disconnect(true);

        return _success;
    }

    private void MessageSent(object o, MessageSentEventArgs args)
    {
        _success = true;
    }
    
    private string? GetTemplate(EmailTemplate template, string email, string fullname, string encryptedText)
    {
        return template switch
        {
            EmailTemplate.UserInvitation => WelcomeTemplate(email, fullname, encryptedText),
            EmailTemplate.UserResetPassword => ResetPasswordTemplate(fullname, encryptedText),
            EmailTemplate.PasswordChanged => PasswordChangedConfirmationTemplate(fullname, encryptedText),
            _ => null
        };
    }

    private string GetSubject(EmailTemplate template)
    {
        return template switch
        {
            EmailTemplate.UserInvitation => "Welcome to Stine Portal",
            EmailTemplate.UserResetPassword => "Forgot Password for Stine Portal",
            EmailTemplate.PasswordChanged => "Stine Portal Password Successfully changed",
            _ => ""
        };
    }
    
    #region Templates

    private string WelcomeTemplate(string email, string fullname, string encryptedText)
    {
        var text = encryptedText.Replace("/", "%2F").Replace("+", "%2B").Replace("=", "%3D");
        var linkUrl = $"{_frontEnd.BaseUrl}{_frontEnd.WelcomeComponent}?token={text}&page=Confirm";
        
        var body = $"<p>" +
                   $"    <img src=\"{_parameters.LogoUrl}\" alt=\"\" width=\"161\" height=\"47\" />" +
                   $"</p>" +
                   $"<p>&nbsp;</p>" +
                   $"<p>{fullname},</p>" +

                   $"<p>Welcome to Stine Portal!</p>" +

                   $"<p style=\"margin-top:25px;\">" +
                   $"    Sign in to your account using the following credentials:" +
                   $"</p>" +
                   $"<p style=\"padding-left: 40px; margin-top:-10px;\">" +
                   $"    Email: {email}" +
                   $"</p>" +

                   $"<p style=\"margin-top:25px;\">" +
                   $"    Upon the first login, it will prompt you to set up your password." +
                   $"</p>" +

                   $"<table style=\"border-collapse: collapse; width: 175px; height: 50px; background-color:#009641\" border=\"0\">" +
                   $"    <tbody>" +
                   $"        <tr style=\"height: 54px;\">" +
                   $"            <td style=\"width: 100%; text-align: center; height: 54px;\">" +
                   $"                <h3>" +
                   $"                    <strong><a style=\"color: #ffffff;\" href=\"{linkUrl}\">Login</a></strong>" +
                   $"                </h3>" +
                   $"            </td>" +
                   $"        </tr>" +
                   $"    </tbody>" +
                   $"</table>";

        return body;
    }

    private string ResetPasswordTemplate(string fullname, string encryptedText)
    {
        var text = encryptedText.Replace("/", "%2F").Replace("+", "%2B").Replace("=", "%3D");
        var linkUrl = $"{_frontEnd.BaseUrl}{_frontEnd.ResetPasswordComponent}?token={text}&page=Forgot";
        
        var body = $"<p>" +
                   $"    <img src=\"https://www.stineseed.com/corntour/images/logos/stine-logo.png\" alt=\"\" width=\"161\" height=\"47\" />" +
                   $"</p>" +
                   $"<p>&nbsp;</p>" +
                   $"<p>{fullname},</p>" + 
                   $"<p style=\"margin-top: 25px;\">" +
                   $"    Follow this link to reset your password for Stine Portal." +
                   $"</p>" +
                   $"<p style=\"margin-top: 25px;\">" +
                   $"    If you did not request a new password, you can safely delete this email." +
                   $"</p>" +
                   $"<table style=\"border-collapse: collapse; width: 350px; height: 50px;\" border=\"0\">" +
                   $"    <tbody>" +
                   $"        <tr style=\"height: 54px;\">" +
                   $"            <td style=\"width: 210px; text-align: center; height: 54px;  background-color: #009641;\">" +
                   $"                <h3>" +
                   $"                    <strong><a style=\"color: #ffffff;\" href=\"{linkUrl}\">Reset your Password</a></strong>" +
                   $"                </h3>" +
                   $"            </td>" + 
                   $"            <td style=width:10px;>" +
                   $"            </td>" +
                   $"            <td style=\"width:130px; text-align: center;\">" +
                   $"                Or <a href=\"{_frontEnd.BaseUrl}\">Go to Stine Portal</a>" +
                   $"            </td>" +
                   $"        </tr>" +
                   $"    </tbody>" +
                   $"</table>";

        return body;
    }

    private string PasswordChangedConfirmationTemplate(string fullname, string encryptedText)
    {
        var text = encryptedText.Replace("/", "%2F").Replace("+", "%2B").Replace("=", "%3D");
        var linkUrl = $"{_frontEnd.BaseUrl}{_frontEnd.ResetPasswordComponent}?token={text}&page=Forgot";
        
        var body = $"<p>" +
                   $"    <img src=\"https://www.stineseed.com/corntour/images/logos/stine-logo.png\" alt=\"\" width=\"161\" height=\"47\" />" +
                   $"</p>" +
                   $"<p>&nbsp;</p>" +
                   $"<p>{fullname},</p>" + 
                   $"<p style=\"margin-top: 25px;\">" +
                   $"     You have successfully changed your password." +
                   $"</p>" +
                   $"<p style=\"margin-top: 25px;\">" +
                   $"     If you did not request a new password, follow this link to reset your password for Stine Portal." +
                   $"</p>" +
                   $"<p>&nbsp;</p>" +
                   $"<table style=\"border-collapse: collapse; width: 350px; height: 50px;\" border=\"0\">" +
                   $"    <tbody>" +
                   $"        <tr style=\"height: 54px;\">" +
                   $"            <td style=\"width: 210px; text-align: center; height: 54px;  background-color: #009641;\">" +
                   $"                <h3>" +
                   $"                    <strong><a style=\"color: #ffffff;\" href=\"{linkUrl}\">Reset your Password</a></strong>" +
                   $"                </h3>" +
                   $"            </td>" + 
                   $"            <td style=width:10px;>" +
                   $"            </td>" +
                   $"            <td style=\"width:130px; text-align: center;\">" +
                   $"                Or <a href=\"{_frontEnd.BaseUrl}\">Go to Stine Portal</a>" +
                   $"            </td>" +
                   $"        </tr>" +
                   $"    </tbody>" +
                   $"</table>";

        return body;
    }
    
    #endregion
}