using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using CM.Model.Dto;
using Microsoft.AspNetCore.Components.Authorization;


namespace CM.App.Helper.State;

public class CustomAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly HttpClient _client;
    private readonly ISyncLocalStorageService _localStorage;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    public CustomAuthenticationStateProvider(HttpClient client, ISyncLocalStorageService localStorage)
    {
        _client = client;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = _localStorage.GetItem<string>("Token");

        if (string.IsNullOrEmpty(token))
        {
            var anonymousState = new AuthenticationState(_anonymous);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
            
            return anonymousState;
        }
        
        var claims = ParseClaimsFromJwt(token).ToList();

        // ToDo: to be deleted!!!
        foreach (var claim in claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
            if (claim.Type == "exp")
            {
                var expireDateTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(claim.Value));
                Console.WriteLine($"Expire: {expireDateTime}");
            }
        }
        
        var identity = new ClaimsIdentity(claims, "jwt");

        var user = new ClaimsPrincipal(identity);

        var state = new AuthenticationState(user);
        
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!
            .Select(k =>
                new Claim(
                    k.Key,
                    k.Value.ToString()!
                    )
            );
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: 
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}