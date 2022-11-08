using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace CM.App.Helper.State;

public class ApiAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly HttpClient _client;
    private readonly ILocalStorageService _localStorage;

    public ApiAuthenticationStateProvider(HttpClient client, ILocalStorageService localStorage)
    {
        _client = client;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("Token");
        var expires = await _localStorage.GetItemAsync<DateTime>("Expires");

        if (string.IsNullOrWhiteSpace(token) || expires < DateTime.UtcNow)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return state;
    }
    
    public void MarkUserAsAuthenticated(string email)
    {
        var authenticatedUser =
            new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email)
            }, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }
    
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        List<Claim> claims = new();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        object roles;
        if (keyValuePairs == null)
            roles = null!;
        else
            keyValuePairs.TryGetValue(ClaimTypes.Role, out roles!);

        if (roles != null!)
        {
            if ((roles.ToString() ?? "") .Trim().StartsWith("["))

            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString() ?? "");
                claims.AddRange(parsedRoles!.Select(parsedRole => new Claim(ClaimTypes.Role, parsedRole)));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString() ?? ""));
            }

            keyValuePairs!.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? "")));

        return claims;
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