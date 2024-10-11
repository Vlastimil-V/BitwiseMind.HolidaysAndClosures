using BitwiseMind.Globalization.TimeZones;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BitwiseMind.Globalization;

public class Program
{
    public static async Task Main(string[] args)
    {
        var commonDataPath = Environment.CurrentDirectory;
        Environment.SetEnvironmentVariable("BASEDIR", commonDataPath);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(CreateConfiguration(args))
            .CreateLogger();

        try
        {
            Log.Information("Starting host...");
            using var host = CreateHostBuilder(args).Build();
            var app = host.Services.GetRequiredService<App>();
            await app.RunAsync();

            Console.ReadLine();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Host terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    static IConfiguration CreateConfiguration(string[] args) =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var environmentName = context.HostingEnvironment.EnvironmentName;
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<App>();
                services.AddHttpClient();
                services.AddHolidayServices();
                services.AddClockServices();

                services.AddStackExchangeRedisCache(options =>
                {
                    context.Configuration.Bind(nameof(RedisCacheOptions), options);
                });
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                // Clear default logging providers to use only Serilog
                logging.ClearProviders();
                logging.AddSerilog();
            });
}