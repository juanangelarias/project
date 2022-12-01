using CM.Database.Helpers;
using CM.Model.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CM.Database;

public class CmDbContextFactory: IDesignTimeDbContextFactory<CmDbContext>
{
    public CmDbContext CreateDbContext(string[] args)
    {
        // Get environment
        //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CM.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var optionsBuilder = new DbContextOptionsBuilder<CmDbContext>();
        var connectionString = config.GetConnectionString(nameof(CmDbContext));
        optionsBuilder.UseSqlServer(connectionString!, b =>
            b.MigrationsAssembly("CM.Database"));
        return new CmDbContext(optionsBuilder.Options, new MigrationUserResolverService(), new DateConverterService());
    }
}