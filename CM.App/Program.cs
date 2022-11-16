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
            where TClient: class
            where TImplementation: class, TClient
        {
            builder.Services.AddHttpClient<TClient, TImplementation>(client =>
            {
                client.BaseAddress = apiBaseUri;
            });
        }
        
        // U
        RegisterTypedClient<IUserService, UserService>(BaseAddress);

        #endregion

        #region State Providers

        builder.Services
            // A
            .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
            // G
            .AddScoped<IGeneralStateProvider, GeneralStateProvider>()
            // P
            .AddScoped<IPageHistoryStateProvider, PageHistoryStateProvider>()
            // U
            .AddScoped<IUserStateProvider, UserStateProvider>();

        #endregion

        builder.Services
            .AddAuthorizationCore()
            .AddBlazorContextMenu()
            .AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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