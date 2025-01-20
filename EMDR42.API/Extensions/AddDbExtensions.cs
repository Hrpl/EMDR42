using EMDR42.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EMDR42.API.Extensions;

public static class AddDbExtensions
{
    public static void AddDataBase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration["ConnectionString:DefaultConnection"]
        ));
        builder.Services.AddDbContext<ApplicationDbContext>();
    }
}
