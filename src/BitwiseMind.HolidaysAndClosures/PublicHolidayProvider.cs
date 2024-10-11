#nullable enable
using System.Runtime.CompilerServices;
using System.Text.Json;
using BitwiseMind.Globalization.TimeZones;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace BitwiseMind.Globalization;

internal class PublicHolidayProvider(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory, IDistributedCache cache, ILogger<PublicHolidayProvider> logger)
    : IPublicHolidayProvider
{
    private readonly List<HolidayDescriptor> _emptyHolidayList = [];

    public async IAsyncEnumerable<PublicHoliday> GetPublicHolidaysAsync(int year, string countryCode, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1, nameof(year));
        ArgumentException.ThrowIfNullOrWhiteSpace(countryCode);

        var cacheKey = $"public_holidays_{year}_{countryCode}";
        var jsonContent = await cache.GetStringAsync(cacheKey, cancellationToken);
        var holidayDescriptors = default(List<HolidayDescriptor>);

        if (!string.IsNullOrEmpty(jsonContent))
        {
            logger.LogInformation("Cache hit for public holidays.");
            holidayDescriptors = JsonSerializer.Deserialize<List<HolidayDescriptor>>(jsonContent);

            await foreach (var holiday in CreatePublicHolidaysAsync(holidayDescriptors, cancellationToken))
            {
                yield return holiday;
            }

            yield break;
        }

        var url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/{countryCode}";
        var httpClient = httpClientFactory.CreateClient();

        try
        {
            logger.LogInformation("Requesting public holidays from external API for {Year} and country {CountryCode}.", year, countryCode);
            var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
            holidayDescriptors = JsonSerializer.Deserialize<List<HolidayDescriptor>>(jsonContent) ?? _emptyHolidayList;

            if (holidayDescriptors.Any())
            {
                logger.LogInformation("Caching public holidays for {Year} and country {CountryCode}.", year, countryCode);
                await cache.SetStringAsync(cacheKey, jsonContent, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CalculateCacheExpiration(year, countryCode)
                }, cancellationToken);
            }
        }
        catch (HttpRequestException exception)
        {
            logger.LogError("Request error: {Message}", exception.Message);
        }
        catch (NotSupportedException exception)
        {
            logger.LogError("The content type is not supported: {Message}", exception.Message);
        }
        catch (JsonException exception)
        {
            logger.LogError("Invalid JSON: {Message}", exception.Message);
        }

        await foreach (var holiday in CreatePublicHolidaysAsync(holidayDescriptors, cancellationToken))
        {
            yield return holiday;
        }
    }

    private async IAsyncEnumerable<PublicHoliday> CreatePublicHolidaysAsync(IEnumerable<HolidayDescriptor>? descriptors, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(descriptors);

        foreach (var descriptor in descriptors)
        {
            // Check if the cancellation has been requested and throw an exception to stop execution.
            cancellationToken.ThrowIfCancellationRequested();

            ArgumentNullException.ThrowIfNull(descriptor.LocalName);
            ArgumentException.ThrowIfNullOrWhiteSpace(descriptor.LocalName);

            ArgumentNullException.ThrowIfNull(descriptor.CountryCode);
            ArgumentException.ThrowIfNullOrWhiteSpace(descriptor.CountryCode);

            logger.LogDebug("Creating public holiday object for {HolidayName} in {CountryCode} on {Date}", descriptor.LocalName, descriptor.CountryCode, descriptor.Date);
            var publicHolidayProvider = serviceProvider.GetRequiredKeyedService<IPublicHolidayProvider>(descriptor.CountryCode);
            yield return new PublicHoliday(descriptor.LocalName,  descriptor.CountryCode, (Start: descriptor.Date, End: descriptor.Date), publicHolidayProvider);

            // Yield to avoid blocking
            await Task.Yield();
        }
    }

    private IEnumerable<PublicHoliday> CreatePublicHolidays(IEnumerable<HolidayDescriptor>? descriptors)
    {
        ArgumentNullException.ThrowIfNull(descriptors);

        foreach (var descriptor in descriptors)
        {
            ArgumentNullException.ThrowIfNull(descriptor.LocalName);
            ArgumentException.ThrowIfNullOrWhiteSpace(descriptor.LocalName);

            ArgumentNullException.ThrowIfNull(descriptor.CountryCode);
            ArgumentException.ThrowIfNullOrWhiteSpace(descriptor.CountryCode);

            logger.LogDebug("Creating public holiday object for {HolidayName} in {CountryCode} on {Date}", descriptor.LocalName, descriptor.CountryCode, descriptor.Date);
            var publicHolidayProvider = serviceProvider.GetRequiredKeyedService<IPublicHolidayProvider>(descriptor.CountryCode);
            yield return new PublicHoliday(descriptor.LocalName, descriptor.CountryCode, (Start: descriptor.Date, End: descriptor.Date), publicHolidayProvider);
        }
    }

    public IEnumerable<PublicHoliday> GetPublicHolidays(int year, string countryCode)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1, nameof(year));
        ArgumentException.ThrowIfNullOrWhiteSpace(countryCode);

        var cacheKey = $"public_holidays_{year}_{countryCode}";
        var jsonContent = cache.GetString(cacheKey);
        var holidayDescriptors = default(List<HolidayDescriptor>);

        if (!string.IsNullOrEmpty(jsonContent))
        {
            logger.LogInformation("Cache hit for public holidays.");
            holidayDescriptors = JsonSerializer.Deserialize<List<HolidayDescriptor>>(jsonContent);

            return CreatePublicHolidays(holidayDescriptors);
        }

        var url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/{countryCode}";
        var httpClient = httpClientFactory.CreateClient();

        try
        {
            logger.LogInformation("Requesting public holidays from external API for {Year} and country {CountryCode}.", year, countryCode);
            var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            jsonContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            holidayDescriptors = JsonSerializer.Deserialize<List<HolidayDescriptor>>(jsonContent);

            if (holidayDescriptors != null)
            {
                logger.LogInformation("Caching public holidays for {Year} and country {CountryCode}.", year, countryCode);
                cache.SetString(cacheKey, jsonContent, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = CalculateCacheExpiration(year, countryCode)
                });

                return CreatePublicHolidays(holidayDescriptors);
            }
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Request error: {Message}", exception.Message);
        }
        catch (NotSupportedException exception)
        {
            logger.LogError(exception, "The content type is not supported: {Message}", exception.Message);
        }
        catch (JsonException exception)
        {
            logger.LogError(exception, "Invalid JSON: {Message}", exception.Message);
        }

        return new List<PublicHoliday>();
    }

    private TimeSpan CalculateCacheExpiration(int year, string countryCode)
    {
        var clockService = serviceProvider.GetRequiredKeyedService<ClockService>(countryCode);
        var timeZones = clockService.GetTimeZones();

        var now = DateTime.UtcNow;
        var yearEnd = new DateTime(year, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        var weight = 1.0;
        var list = new List<(TimeSpan Period, double Weight)>();

        foreach (var timeZone in timeZones)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(timeZone);
            var nowLocal = clockService.ToLocal(timeZone, Instant.FromDateTimeUtc(now));
            var yearEndLocal = clockService.ToLocal(timeZone, Instant.FromDateTimeUtc(yearEnd));

            var duration = Period.Between(nowLocal, yearEndLocal, PeriodUnits.Years | PeriodUnits.Days | PeriodUnits.Hours | PeriodUnits.Minutes).ToDuration();
            list.Add((duration.ToTimeSpan(), weight));
        }

        var expirationTime = WeightedAverageCalculator.CalculateWeightedAverage(list);
        logger.LogInformation("Calculated cache expiration for {Year} is: {Years} years, {Months} months, {Days} days, {Hours} hours, {Minutes} minutes", year, expirationTime.Days / 365, (expirationTime.Days % 365) / 30, expirationTime.Days % 30, expirationTime.Hours, expirationTime.Minutes);
        
        return expirationTime;
    }
}