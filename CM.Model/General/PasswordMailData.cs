using CM.Model.Enum;

namespace CM.Model.General;

public class PasswordMailData
{
    public long UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public EmailTemplate Type { get; set; }
}