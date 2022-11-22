global using Microsoft.AspNetCore.Components.Authorization;

using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CM.App.Helper.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CM.App.Helper.State;
using MudBlazor.Services;

namespace Cm.App;

public static class Program
{
    private static readonly Uri BaseAddress = new Uri("https://localhost:7213/");

    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<CM.App.App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        #region HTTP Clients

        void RegisterTypedClient<TClient, TImplementation>(Uri apiBaseUri)
            where TClient : class
            where TImplementation : class, TClient
        {
            builder.Services.AddHttpClient<TClient, TImplementation>(client =>
            {
                client.BaseAddress = apiBaseUri;
            });
        }
        // C
        RegisterTypedClient<IClubService, ClubService>(BaseAddress);
        RegisterTypedClient<IClubTypeService, ClubTypeService>(BaseAddress);
        RegisterTypedClient<IConferenceService, ConferenceService>(BaseAddress);
        RegisterTypedClient<ICountryService, CountryService>(BaseAddress);
        RegisterTypedClient<ICurrencyService, CurrencyService>(BaseAddress);
        // M
        RegisterTypedClient<IMemberService, MemberService>(BaseAddress);
        // R
        RegisterTypedClient<IRoleService, RoleService>(BaseAddress);
        // U
        RegisterTypedClient<IUserService, UserService>(BaseAddress);

        #endregion

        #region State Providers

        builder.Services
            // A
            .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
            // C
            .AddScoped<IClubStateProvider, ClubStateProvider>()
            .AddScoped<IClubTypeStateProvider, ClubTypeStateProvider>()
            .AddScoped<IConferenceStateProvider, ConferenceStateProvider>()
            .AddScoped<ICountryStateProvider, CountryStateProvider>()
            .AddScoped<ICurrencyStateProvider, CurrencyStateProvider>()
            // G
            .AddScoped<IGeneralStateProvider, GeneralStateProvider>()
            // M
            .AddScoped<IMemberStateProvider, MemberStateProvider>()
            // P
            .AddScoped<IPageHistoryStateProvider, PageHistoryStateProvider>()
            // R
            .AddScoped<IRoleStateProvider, RoleStateProvider>()
            // U
            .AddScoped<IUserStateProvider, UserStateProvider>();

        #endregion

        builder.Services
            .AddAuthorizationCore()
            .AddBlazorContextMenu()
            .AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            })
            .AddBlazoredSessionStorage()
            .AddMudServices();

        await builder.Build().RunAsync();
    }
}
