using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.IO;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Warning()
                .WriteTo
                .Debug(new RenderedCompactJsonFormatter())
                .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("Starting host.");
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex) 
            {
                Log.Fatal(ex, "Host terminated.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://0.0.0.0:5000");
                    webBuilder.UseStartup<Startup>();
                });
        }

    }
}
