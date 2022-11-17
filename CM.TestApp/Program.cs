using AutoMapper;
using AutoMapper.EquivalencyExpression;
using CM.Common.Configuration.Models;
using CM.Core.Authentication;
using CM.Core.Middleware;
using CM.Core.Services.Encryption;
using CM.Core.Services.Mail;
using CM.Database;
using CM.Database.Helpers;
using CM.Database.Mappings;
using CM.Features;
using CM.Model.Services;
using CM.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CM.TestApp;

public static class Program
{
    private static IHost _host;
    private static IConfiguration? _configuration;
    
    public static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", optional: false);

        _configuration = builder.Build();
        
        var keyConfig = new Keys();
        _configuration.Bind("Keys", keyConfig);

        var mailParameter = new MailParameter();
        _configuration.Bind("MailParameters", mailParameter);

        var uspsConfig = new UspsConfig();
        _configuration.Bind("USPSServiceConfig", uspsConfig);

        var frontEndParameter = new FrontEndParameters();
        _configuration.Bind("FrontEndParameters", frontEndParameter);

        var passwordSettings = new PasswordSettings();
        _configuration.Bind("PasswordSettings", passwordSettings);

        _host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services
                    .AddAutoMapper((serviceProvider, automapper) =>
                    {
                        automapper.AddCollectionMappers();
                        automapper.UseEntityFrameworkCoreModel<CmDbContext>(serviceProvider);
                        automapper.AddProfile(typeof(SqlMappingProfile));
                    }, typeof(CmDbContext).Assembly)
                    .AddDbContext<CmDbContext>(options =>
                    {
                        options.UseSqlServer(_configuration.GetConnectionString(nameof(CmDbContext)));
                    })
                    .AddSingleton(keyConfig)
                    .AddSingleton(mailParameter)
                    .AddSingleton(uspsConfig)
                    .AddSingleton(frontEndParameter)
                    .AddSingleton(passwordSettings)
                    // C
                    .AddScoped<ICountryRepository, CountryRepository>()
                    
                    // D
                    .AddScoped<IDateConverterService, DateConverterService>()

                    // E
                    .AddScoped<IEncryptionService, EncryptionService>()

                    // J
                    .AddScoped<IJwtGenerator, JwtGenerator>()
                    .AddScoped<JwtMiddleware>()

                    // M
                    .AddScoped<IMailService, MailService>()

                    // R
                    .AddScoped<IRoleRepository, RoleRepository>()

                    // U
                    .AddScoped<IUserFeature, UserFeature>()
                    .AddScoped<IUserPasswordRepository, UserPasswordRepository>()
                    .AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IUserRequestTokenRepository, UserRequestTokenRepository>()
                    .AddScoped<IUserResolverService, MinimumUserResolverService>() 
                            // ToDo: Change to UserResolveService once User functionality is ready
                    .AddScoped<IUserRoleRepository, UserRoleRepository>();
            })
            .Build();
        
        await Start();
    }

    private static async Task Start()
    {
        var seeder = new Seeder(_host.Services.GetService<ICountryRepository>());

        await seeder.SeedCountry();

        Console.ReadLine();
    }

    private static void EncryptionTest()
    {
        var tools = new Tools(
            _host.Services.GetService<IUserFeature>(), 
            _host.Services.GetService<IEncryptionService>());

        var key = tools.GetKey();
        var plainText = "Delfos02!!";
        Console.WriteLine($"Key:         {key}");
        Console.WriteLine($"Plain text:  {plainText}");
        var hashedText = tools.Encrypt(plainText, key);
        var plainText2 = tools.Decrypt(hashedText, key);
        Console.WriteLine($"Hashed text: {hashedText}");
        Console.WriteLine($"Plain text:  {plainText}");

        Console.ReadLine();
    }
    
    
}