using Application;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Globalization;

namespace API {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

      services.AddControllers().AddNewtonsoftJson(options =>
      {
        options.SerializerSettings.DateFormatString = "MM/dd/yyyy";
      });

      services.AddApplication();
      services.AddInfrastructure(Configuration);

      services.AddCors(options => {
        options.AddPolicy("Default", builder => {
          builder.AllowAnyOrigin();
          builder.AllowAnyMethod();
          builder.AllowAnyHeader();
        });
      });

      services.AddMemoryCache();
      services.Configure<RequestLocalizationOptions>(options => {
        var supportedCultures = new List<CultureInfo> {
            new("en-US"),
            new("uk-UA"),
            new("ru-RU")
        };

        options.DefaultRequestCulture = new RequestCulture("uk-UA");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
      });

      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
          In = ParameterLocation.Header,
          Description = "Please insert JWT with Bearer into field",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      System.Array.Empty<string>()
    }
  });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeeder seeder) {

      seeder.Seed().GetAwaiter().GetResult();

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
      }

      var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
      app.UseRequestLocalization(localizationOptions.Value);

      app.UseCors("Default");
      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}
