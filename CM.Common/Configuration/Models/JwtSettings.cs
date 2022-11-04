namespace CM.Common.Configuration.Models;

public class JwtSettings
{
    public int RefreshTokenTTLInDay { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Key { get; set; }
    public int AccessTokenValidityInMinutes { get; set; }
    public int RefreshTokenValidityInMinutes { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
}