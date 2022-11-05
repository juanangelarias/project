﻿using System.Text;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using CM.Common;
using CM.Common.Configuration.Models;
using CM.Common.Middleware;
using CM.Database;
using CM.Database.Mappings;
using CM.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Environments = CM.Common.Enum.Environments;

namespace CM.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper((serviceProvider, automapper) =>
        {
            automapper.AddCollectionMappers();
            automapper.UseEntityFrameworkCoreModel<CmDbContext>(serviceProvider);
            automapper.AddProfile(typeof(SqlMappingProfile));
        }, typeof(CmDbContext).Assembly);
        
        services.AddControllers()
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        services.AddHttpContextAccessor();

        services.AddDbContext<CmDbContext>(options =>
        {
            options.UseSqlServer(_configuration.GetConnectionString(nameof(CmDbContext)));
        });
        
        #region Swagger
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Description = "Stine Portal API",
                Title = "StinePortal.Api",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization Header using Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        
        #endregion

        #region Configs

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

        services
            .AddSingleton(keyConfig)
            .AddSingleton(mailParameter)
            .AddSingleton(uspsConfig)
            .AddSingleton(frontEndParameter)
            .AddSingleton(passwordSettings);

        #endregion
        
        #region Auth

        services.AddAuthorization();

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
            });

        #endregion

        #region CORS
        
        var corsSettings = _configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicies.Development, builder =>
            {
                builder.SetIsOriginAllowed(_ => true);
                builder.WithOrigins(corsSettings.AllowedOrigins);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });

            options.AddPolicy(CorsPolicies.Test, builder =>
            {
                builder.SetIsOriginAllowed(_ => true);
                builder.WithOrigins(corsSettings.AllowedOrigins);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });

            options.AddPolicy(CorsPolicies.Uat, builder =>
            {
                builder.SetIsOriginAllowed(_ => true);
                builder.WithOrigins(corsSettings.AllowedOrigins);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });
        });
        
        #endregion

        services.Configure<FormOptions>(options =>
        {
            // Set the limit to 256 MB
            options.MultipartBodyLengthLimit = 268435456;
        });

        // Add your Config object so it can be injected
        services.Configure<JwtSettings>(_configuration.GetSection("JwtSettings"));

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        #region IOC container registrations

        services
            // R
            .AddScoped<IRoleRepository, RoleRepository>()
            
            // U
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserRoleRepository, UserRoleRepository>();

        #endregion
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsEnvironment(Environments.Test))
        {
            app.UseDeveloperExceptionPage();
            app.UseSerilogRequestLogging();
        }

        app.UseSwagger();
        app.UseSwaggerUI(opts =>
        {
            opts.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            opts.SwaggerEndpoint("/swagger/v1/swagger.json", "CmPortalApi v1");
        });

        app.UseHttpsRedirection();
        app.UseRouting();

        if (env.IsEnvironment(Environments.Test))
        {
            app.UseCors(CorsPolicies.Test);
        }
        else if (env.IsEnvironment(Environments.Uat))
        {
            app.UseCors(CorsPolicies.Uat);
        }
        else if (env.IsDevelopment())
        {
            app.UseCors(CorsPolicies.Development);
        }
        else
        {
            app.UseCors(CorsPolicies.Development);
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<JwtMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}