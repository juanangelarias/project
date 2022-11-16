using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using CM.Model.General;
using Microsoft.AspNetCore.Components.Authorization;

namespace CM.App.Helper.State;

public class GeneralStateProvider : IGeneralStateProvider
{
    /*private const string JwtKey = "IGggo82wXEi0SYyQTGU7LgLocal";*/
    
    private readonly ISyncLocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authentication;

    private DateTime? _expires;
    private string? _token;
    private long? _userId;
    private string? _username;
    private string? _userFullName;
    private List<string>? _roles;
    //private ConferenceDto? _conference;

    public DateTime? Expires
    {
        get => _expires;
        set
        {
            _expires = value;
            NotifyChanges();
        }
    }

    public string? Token
    {
        get => _token;
        set
        {
            _token = value;
            NotifyChanges();
        }
    }

    public long? UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            NotifyChanges();
        }
    }

    public string? Username
    {
        get => _username;
        set
        {
            _username = value;
            NotifyChanges();
        }
    }

    public string? UserFullName
    {
        get => _userFullName;
        set
        {
            _userFullName = value;
            NotifyChanges();
        }
    }

    public List<string>? Roles
    {
        get => _roles;
        set
        {
            _roles = value;
            NotifyChanges();
        }
    }
    
    /*public ConferenceDto? Conference
    {
        get => _conference;
        set
        {
            _conference = value;
            _localStorage.SetItem("Conference", JsonSerializer.Serialize(value));
            NotifyChanges();
        }
    }*/

    
    public Action? OnChanges;

    public void Initialize()
    {
        SetData();
    }
    
    public GeneralStateProvider(
        ISyncLocalStorageService localStorage, 
        AuthenticationStateProvider authentication)
    {
        _localStorage = localStorage;
        _authentication = authentication;

        SetData();
    }
    
    public void SetLocalStorage(LoginResponse data)
    {
        _localStorage.SetItem("Token", data.Token);
        //_localStorage?.SetItem("Conference", JsonSerializer.Serialize(data.Conference));
        _localStorage.SetItem("User", JsonSerializer.Serialize(data.User));
        
        SetData();
    }

    public void Logout()
    {
        _localStorage.Clear();
        SetData();
    }
    
    private void SetData()
    {
        _token = _localStorage.ContainKey("Token")
            ? _localStorage.GetItem<string>("Token")
            : null;

        if (_token == null)
            return;

        var claims = Tools.ParseClaimsFromJwt(_token).ToList();

        foreach (var claim in claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }
        _userId = Convert.ToInt64(claims.FirstOrDefault(r => r.Type == "Id")?.Value);
        _username = claims.FirstOrDefault(r => r.Type == "UserName")?.Value;
        _userFullName = claims.FirstOrDefault(r => r.Type == "FullName")?.Value;
        _roles = claims
            .Where(r => r.Type == ClaimTypes.Role)
            .Select(s => s.Value)
            .ToList();

        /*_conference = _localStorage.ContainKey("Conference")
            ? JsonSerializer.Deserialize<ConferenceDto>(_localStorage.GetItem<string>("Conference"))
            : null;*/

        _authentication.GetAuthenticationStateAsync();

        NotifyChanges();
    }

    /*private void ReadToken(string token)
    {
        Token = token;
        
        var key = Encoding.ASCII.GetBytes(JwtKey);
        var handler = new JwtSecurityTokenHandler();
        var validations = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        var claims = handler.ValidateToken(token, validations, out var tokenSecure);
        
        //User =
        //return claims.Identity.Name;
    }*/
    
    private void NotifyChanges() => OnChanges?.Invoke();
}