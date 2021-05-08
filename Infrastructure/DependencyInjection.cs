using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure {
  public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
      services.AddDbContext<DeratControlDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("DeratControlDatabase")));

      services.AddScoped<IDeratControlDbContext, DeratControlDbContext>();

      services.AddSingleton<IFileStorage, FileStorage>();
      services.AddSingleton<ICurrentDateService, MachineDateService>();

      services
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters {
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
}
