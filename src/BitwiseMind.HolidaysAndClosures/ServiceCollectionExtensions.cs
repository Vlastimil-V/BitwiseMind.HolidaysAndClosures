using Microsoft.Extensions.DependencyInjection;

namespace BitwiseMind.Globalization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHolidayServices(this IServiceCollection services) => 
        services.AddHolidayServices(Countries.All);

    public static IServiceCollection AddHolidayServices(this IServiceCollection services, IEnumerable<string> countries)
    {
        // Register for all countries HolidayManager, ClosedDaysManager and IPublicHolidayProvider implementations
        var currentYear = DateTime.Now.Year;
        foreach (var country in countries)
        {
            services.AddKeyedTransient<IPublicHolidayProvider>(country, (serviceProvider, _) => 
                ActivatorUtilities.CreateInstance<PublicHolidayProvider>(serviceProvider));

            services.AddKeyedTransient<IClosedDaysManager>(country, (serviceProvider, serviceKey) =>
            {
                var holidaysProvider = serviceProvider.GetRequiredKeyedService<IPublicHolidayProvider>((string)serviceKey!);
                return new ClosedDaysManager(holidaysProvider.GetPublicHolidaysAsync(currentYear, country));
            });
        }

        return services;
    }
}