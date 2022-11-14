using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CM.App.Helper.State;
using CM.Model.Dto;
using CM.Model.Enum;
using CM.Model.General;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CM.App.Helper.Services;

public class UserService: BaseService<UserDto>, IUserService
{
    private readonly NavigationManager _navigation;
    private readonly IGeneralStateProvider _state;
    private readonly HttpClient _client;
    private readonly ISnackbar _snackbar;

    public UserService(NavigationManager navigation, IGeneralStateProvider state, HttpClient client, ISnackbar snackbar)
        : base(navigation, state, client, snackbar)
    {
        _navigation = navigation;
        _state = state;
        _client = client;
        _snackbar = snackbar;

        BaseUrl = "api/user/v1";
    }
    
    /*public async Task<List<ConferenceDto>?> GetUserConferences(long userId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{userId}/conferences");
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<List<ConferenceDto>>();
            return result;
        }

        return null;
    }*/
    
    public async Task<LoginResponse?> Login(LoginData login)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/login")
        {
            Content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json")
        };
        var response = await GetResponse(request);
        
        if (response == null) 
            return null;
        
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return result;
    }

    public async Task<bool?> ResetPassword(ResetPassword data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/resetPassword")
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        return null;
    }

    public async Task<bool?> ResetForgotPassword(string username, string newPassword)
    {
        var data = new ResetPassword
        {
            UserName = username,
            Password = null!,
            NewPassword = newPassword
        };
        
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/resetForgotPassword")
        {
            Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };
        var response = await GetResponse(request);
        if (response != null)
        {
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }

        return null;
    }

    public async Task AddRoleToUser(UserRoleDto userRole)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/addRoleToUser")
        {
            Content = new StringContent(JsonSerializer.Serialize(userRole), Encoding.UTF8, "application/json")
        };
        await GetResponse(request);
    }

    public async Task RemoveRoleFromUser(long userRoleId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/removeRoleFromUser/{userRoleId}");
        await GetResponse(request);
    }
    
    /*public async Task SetDefaultConference(long userId, ConferenceDto conference)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/{userId}/setDefaultConference")
        {
            Content = new StringContent(JsonSerializer.Serialize(conference), Encoding.UTF8, "application/json")
        };
        await GetResponse(request);
    }*/

    public async Task<bool> SendInvitation(long userId)
    {
        const int template = (int) EmailTemplate.UserInvitation;
        
        var result = false;
        
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{BaseUrl}/{userId}/sendMail/{template}");

        var response = await GetResponse(request);
        if (response != null)
        {
            result = await response.Content.ReadFromJsonAsync<bool>();
        }

        var msg = result
            ? "Email sent successfully"
            : "Email was not sent!";

        var svr = result
            ? Severity.Info
            : Severity.Error;

        _snackbar.Add(msg, svr);
        
        return result;
    }

    public async Task<PasswordMailData> GetUserFromToken(string token)
    {
        PasswordMailData result = null;
        var request = new HttpRequestMessage(
            HttpMethod.Get, 
            $"{BaseUrl}/getUserFromToken?token={token}");

        var response = await GetResponse(request);
        if (response != null)
        {
            result = await response.Content.ReadFromJsonAsync<PasswordMailData>();
        }

        return result;
    }
}