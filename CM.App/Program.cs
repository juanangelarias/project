using Blazored.LocalStorage;
using CM.App.Helper.Services;
using CM.App.Helper.State;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace CM.App;

public class Program
{
    private static readonly Uri BaseAddress = new Uri("https://localhost:7213");
    
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddMudServices();

        void RegisterTypedClient<TClient, TImplementation>(Uri apiBaseUri)
            where TClient : class
            where TImplementation : class, TClient
        {
            builder.Services.AddHttpClient<TClient, TImplementation>(client =>
            {
                client.BaseAddress = apiBaseUri;
            });
        }

        #region Http Clients

        // U
        RegisterTypedClient<IUserService, UserService>(BaseAddress);

        #endregion

        #region State Provider

        builder.Services
            // A
            .AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>()
            
            // G
            .AddScoped<IGeneralStateProvider, GeneralStateProvider>()
            
            // P
            .AddScoped<IPageHistoryStateProvider, PageHistoryStateProvider>()
            
            // U
            .AddScoped<IUserStateProvider, UserStateProvider>();

        #endregion

        builder.Services
            .AddAuthorizationCore()
            .AddBlazoredLocalStorage()
            .AddMudServices();

        await builder.Build().RunAsync();
    }
}