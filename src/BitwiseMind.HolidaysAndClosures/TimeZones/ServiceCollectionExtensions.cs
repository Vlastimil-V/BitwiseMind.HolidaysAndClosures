using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NodaTime;

namespace BitwiseMind.Globalization.TimeZones;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClockServices(this IServiceCollection services)
    {
        var timeZoneMapping = TimeZoneMappingProvider.GetCountryTimeZones();
        services.TryAddSingleton(timeZoneMapping);

        // Register for all countries a ClockService per available timezone
        foreach (var (name, countryCode, zones) in timeZoneMapping.Countries.Values)
        {
            services.AddKeyedTransient<ClockService>(countryCode, (serviceProvider, serviceKey) =>
            {
                ArgumentException.ThrowIfNullOrWhiteSpace((string?)serviceKey);

                var mapping = serviceProvider.GetRequiredService<TimeZoneMapping>();
                var timeZones = mapping.FindTimeZonesByCountryCode((string)serviceKey);

                TimeZoneClockService GetTimeZoneClockService(string timeZone) =>
                    serviceProvider.GetRequiredKeyedService<TimeZoneClockService>(timeZone);

                var timeZoneClockServices = timeZones
                    .Select(GetTimeZoneClockService)
                    .ToList();


                return new ClockService((string)serviceKey, timeZoneClockServices);
            });

            zones.ForEach(timeZone =>
                services.AddKeyedTransient<TimeZoneClockService>(timeZone, (serviceProvider, serviceKey) =>
                    new TimeZoneClockService(DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZone), null))
            );

        }

        return services;
    }
}