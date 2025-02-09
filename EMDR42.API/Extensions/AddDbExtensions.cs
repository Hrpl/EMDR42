using EMDR42.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMDR42.API.Extensions;

public static class AddDbExtensions
{
    public static void AddDataBase(this WebApplicationBuilder builder)
    {
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(
            $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};"
        ));
        builder.Services.AddDbContext<ApplicationDbContext>();
    }
}
