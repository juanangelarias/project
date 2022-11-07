namespace CM.Model.General;

public class PasswordMailToken
{
    public long UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}