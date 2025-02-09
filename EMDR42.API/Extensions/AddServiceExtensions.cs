using EMDR42.API.Services.Implementation;
using EMDR42.API.Services.Interfaces;
using EMDR42.Domain.Commons.Singleton;
using EMDR42.Infrastructure.Context;
using EMDR42.Infrastructure.Services.Implementations;
using EMDR42.Infrastructure.Services.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace EMDR42.API.Extensions;

public static class AddServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddMapster();
        services.AddRegisterService();
        services.AddOpenAPI();
    }
    public static void AddOpenAPI(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
            {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Repositories", Version = "v2024" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Authorization using jwt token. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
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
    }
    public static void AddJwt(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var s = Environment.GetEnvironmentVariable("JWT_KEY");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"))),
                ValidateIssuerSigningKey = true
            };
        });
        builder.Services.AddAuthorization();
    }
    public static void AddMapster(this IServiceCollection services)
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        Mapper mapperConf = new(config);
        services.AddSingleton<IMapper>(mapperConf);
    }
    public static void AddRegisterService(this IServiceCollection services)
    {
        services.AddSingleton<Config>();
        services.AddScoped<IDbConnectionManager, DbConnectionManager>();
        services.AddScoped<IJwtHelper, JwtHelper>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IQualificationRepository, QualificationRepository>();
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IFindSpecialistRepository, FindSpecialistRepository>();
        services.AddScoped<ITherapyRepository, TherapyRepository>();
    }
}
