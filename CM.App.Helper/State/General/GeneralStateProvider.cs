using System.Text.Json;
using Blazored.LocalStorage;
using CM.Model.Dto;
using CM.Model.General;
using Microsoft.AspNetCore.Components.Authorization;

namespace CM.App.Helper.State;

public class GeneralStateProvider : IGeneralStateProvider
{
    private readonly ISyncLocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authentication;

    private DateTime? _expires;
    private string? _token;
    //private ConferenceDto? _conference;
    private UserDto? _user;
    //private List<RoleDto>? _roles;
    

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

    public UserDto? User
    {
        get => _user;
        set
        {
            _user = value;
            NotifyChanges();
        }
    }

    /*public List<RoleDto>? Roles
    {
        get => _roles;
        set
        {
            _roles = value;
            NotifyChanges();
        }
    }*/

    public Action? OnChanges;

    public void Initialize()
    {
        SetData();
    }
    
    public GeneralStateProvider(ISyncLocalStorageService localStorage, AuthenticationStateProvider authentication)
    {
        _localStorage = localStorage;
        _authentication = authentication;

        SetData();
    }
    
    public void SetLocalStorage(LoginResponse data)
    {
        _localStorage.SetItem("Expires", data.Expires);
        _localStorage.SetItem("Token", data.Token);
        //_localStorage?.SetItem("Conference", JsonSerializer.Serialize(data.Conference));
        _localStorage.SetItem("User", JsonSerializer.Serialize(data.User));
        //_localStorage?.SetItem("Roles", JsonSerializer.Serialize(data.Roles));
        
        SetData();
    }

    public void Logout()
    {
        _localStorage.Clear();
        SetData();
        _authentication.GetAuthenticationStateAsync();
    }
    
    private void SetData()
    {
        _expires = _localStorage.ContainKey("Expires")
            ? _localStorage.GetItem<DateTime?>("Expires")
            : null;

        _token = _localStorage.ContainKey("Token")
            ? _localStorage.GetItem<string>("Token")
            : null;

        _user = _localStorage.ContainKey("User")
            ? JsonSerializer.Deserialize<UserDto>(_localStorage.GetItem<string>("User"))
            : null;

        /*_roles = _localStorage.ContainKey("Roles")
            ? JsonSerializer.Deserialize<List<RoleDto>>(_localStorage.GetItem<string>("Roles"))
            : null;*/

        /*_conference = _localStorage.ContainKey("Conference")
            ? JsonSerializer.Deserialize<ConferenceDto>(_localStorage.GetItem<string>("Conference"))
            : null;*/

        NotifyChanges();
    }
    
    private void NotifyChanges() => OnChanges?.Invoke();
}