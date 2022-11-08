namespace CM.Common.Configuration.Models;

public class MailParameter
{
    public string? From { get; set; }
    public string? DisplayName { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? SmtpServer { get; set; }
    public int? Port { get; set; }
    public bool? EnableSsl { get; set; }
    public string? LogoUrl { get; set; }
}