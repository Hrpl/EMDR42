﻿using EMDR42.API.Services.Implementation;
using EMDR42.API.Services.Interfaces;
using EMDR42.Domain.Commons.Options;
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
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = builder.Configuration["JwtConfigurations:Issuer"],
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfigurations:Key"])),
                ValidateIssuerSigningKey = true
            };
        });
        builder.Services.AddAuthorization();
    }

    public static void AddOptionsSmtp(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.Configure<SmtpClientOptions>(builder.Configuration.GetSection(SmtpClientOptions.Key));
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
        services.AddScoped<IDbConnectionManager, DbConnectionManager>();
        services.AddScoped<IJwtHelper, JwtHelper>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IQualificationService, QualificationService>();
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<IClientService, ClientService>();
    }
}
