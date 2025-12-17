using Serilog;
using Serilog.Events;

namespace Restaurants.API.Extenstions;

public static class ConfigureHostBuilderExtension
{
    public static void AddSerilogConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((_, configuration) =>
        {
            configuration
                .MinimumLevel.Information() 
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information) 
                .Enrich.FromLogContext()
                .WriteTo.File("logs/Restaurant-API-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Logger(consoleLogger =>
                {
                    consoleLogger
                        .Filter.ByExcluding(logEvent =>
                        {
                            if (logEvent.Level == LogEventLevel.Information)
                            {
                                if (logEvent.Properties.TryGetValue("SourceContext", out var value))
                                {
                                    return value.ToString().Contains("Microsoft.EntityFrameworkCore");
                                }
                            }
                            return false;
                        })
                        .WriteTo.Console();
                });
        });
    }
}