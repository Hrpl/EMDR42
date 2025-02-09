using EMDR42.API.Extensions;
using DotNetEnv;
using Microsoft.OpenApi.Models;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);
builder.AddDataBase();
builder.AddJwt();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddServices();
builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(o => o.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
