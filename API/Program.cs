using API.Filters;
using API.Middleware;
using Application;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new ApiExceptionFilter());
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", builder =>
    {
        builder.WithOrigins("http://localhost:4200");
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new("en-US")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddHealthChecks();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddScoped<UserInitializer>();

var app = builder.Build();

app.MapHealthChecks("status");

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<DeratControlDbContext>();
var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

await db.Database.MigrateAsync();

await seeder.Seed();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

app.UseCors("Default");
app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<UserInitializer>();

app.MapControllers();

app.Run();