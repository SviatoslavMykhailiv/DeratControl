using System;
using Application.Common.Interfaces;
using DeratControl.Infrastructure.Services.Reports;
using DinkToPdf;
using DinkToPdf.Contracts;
using Infrastructure.Identity;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DeratControlDbContext>(
            b =>
                b.UseMySql(
                    configuration.GetConnectionString("DeratControlDatabase"), new MySqlServerVersion(new Version(8, 0, 41)),
                    options => options.MigrationsAssembly("API")));
        
        services.AddTransient<IDeratControlDbContext, DeratControlDbContext>();

        services.AddSingleton<IFileStorage, FileStorage>();
        services.AddSingleton<ICurrentDateService, MachineDateService>();

        services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = AuthService.USER_NAME_CLAIM,
                        RoleClaimType = AuthService.USER_ROLE_CLAIM,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateActor = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Security:SecurityKey"]))
                    };
                });

        services.AddIdentity<ApplicationUser, ApplicationRole>()
          .AddEntityFrameworkStores<DeratControlDbContext>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserManagerService, UserManagerService>();
        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        services.AddScoped<IReportBuilder, HTMLReportBuilder>();

        services.AddOptions<EncryptionOptions>().Configure(options =>
        {
            options.IV = [255, 64, 191, 111, 23, 3, 113, 119, 231, 121, 252, 112, 79, 32, 114, 156];
            options.Key = Encoding.ASCII.GetBytes(configuration["Security:QrCodeSymmetricEncryptionKey"]);
        });

        services.AddSingleton<IEncryptionService, AESEncryptionService>();
        services.AddSingleton<IQRCodeService, QRCodeService>();
        services.AddSingleton<IQRListGenerator, QRListGenerator>();

        services.AddOptions<AuthOptions>().Configure(c =>
        {
            c.LifeTime = int.Parse(configuration["Security:LifeTime"]);
            c.SecurityKey = configuration["Security:SecurityKey"];
            c.QrCodeSymmetricEncryptionKey = configuration["Security:QrCodeSymmetricEncryptionKey"];
        });

        services.AddScoped<DataSeeder>();

        return services;
    }
}
