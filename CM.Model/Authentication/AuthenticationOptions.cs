namespace CM.Model.Authentication;

public class AuthenticationOptions
{
    public string? Authority { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public string? ApiName { get; set; }
}