using CM.Model.General;

namespace CM.Core.Services.Mail;

public interface IMailService
{
    bool SendEmail(EmailMessage emailMessage);
    bool SendTemplate(PasswordMailData data);
}