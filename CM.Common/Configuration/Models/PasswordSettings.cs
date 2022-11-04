namespace CM.Common.Configuration.Models;

public class PasswordSettings
{
    public int PasswordExpirationInDays { get; set; }
    public int WelcomeTokenExpirationInHours { get; set; }
    public int ResetPasswordTokenExpirationInMinutes { get; set; }
}