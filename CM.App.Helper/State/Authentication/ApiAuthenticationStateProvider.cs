using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using CM.Model.Dto;

namespace CM.App.Helper.State;

public class ApiAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly HttpClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public ApiAuthenticationStateProvider(HttpClient client, ILocalStorageService localStorage)
    {
        _client = client;
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await ReadEncryptedItemAsync<UserDto>("User");
            if (userSession.FullName == null)
                return await Task.FromResult(new AuthenticationState(_anonymous));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.FullName),
                new Claim(ClaimTypes.Role, "Admin")
            }, "JwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationState(UserDto? user)
    {
        ClaimsPrincipal claimsPrincipal;

        if (user != null)
        {
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Name, user.FullName ?? "Guest"),
                new(ClaimTypes.Role, user.Roles?.FirstOrDefault()?.Role?.Code ?? "Guest")
            }));

            await SaveItemEncryptedAsync("User", user);
        }
        else
        {
            claimsPrincipal = _anonymous;
            await _localStorage.RemoveItemAsync("User");
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task<string> GetToken()
    {
        var result = string.Empty;

        try
        {
            var user = await ReadEncryptedItemAsync<UserDto>("User");
            if (user != null || DateTime.Now < user.Expires)
                result = user.Token;
        }
        catch
        { }

        return result;
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
    
    private async Task SaveItemEncryptedAsync<T>(string key, T item)
    {
        var itemJson = JsonSerializer.Serialize(item);
        var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
        var base64Json = Convert.ToBase64String(itemJsonBytes);
        await _localStorage.SetItemAsStringAsync(key, base64Json);
    }
    
    private async Task<T> ReadEncryptedItemAsync<T>(string key)
    {
        var base64Json = await _localStorage.GetItemAsync<string>(key);
        var itemJsonBytes = Convert.FromBase64String(base64Json);
        var itemJson = Encoding.UTF8.GetString(itemJsonBytes);
        var item = JsonSerializer.Deserialize<T>(itemJson);

        return item;
    }
}