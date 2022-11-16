using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;


namespace CM.App.Helper.State;

public class CustomAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly ISyncLocalStorageService _localStorage;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    public CustomAuthenticationStateProvider(ISyncLocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = _localStorage.GetItem<string>("Token");

        if (string.IsNullOrEmpty(token))
        {
            var anonymousState = new AuthenticationState(_anonymous);
            NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
            
            return Task.FromResult(anonymousState);
        }
        
        var claims = Tools.ParseClaimsFromJwt(token).ToList();
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);
        
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return Task.FromResult(state);
    }
}