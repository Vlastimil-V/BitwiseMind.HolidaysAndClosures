using System.Diagnostics.CodeAnalysis;
using NodaTime;

namespace BitwiseMind.Globalization.TimeZones;

public class ClockService([DisallowNull] string? country, [DisallowNull] List<TimeZoneClockService>? timeZoneClockServices)
{
    public DateTimeZone GetTimeZone(string timeZoneId)
    {
        var timeZoneClockService = GetTimeZoneClockService(timeZoneId);
        return timeZoneClockService.TimeZone;
    }

    public DateTimeOffset GetCurrentTime(string timeZoneId)
    {
        var timeZoneClockService = GetTimeZoneClockService(timeZoneId);
        return timeZoneClockService.CurrentTime;
    }

    public LocalDateTime GetLocalNow(string timeZoneId)
    {
        var timeZoneClockService = GetTimeZoneClockService(timeZoneId);
        return timeZoneClockService.LocalNow;
    }

    public Instant ToInstant(string timeZoneId, LocalDateTime local)
    {
        var timeZoneClockService = GetTimeZoneClockService(timeZoneId);
        return timeZoneClockService.ToInstant(local);
    }

    public LocalDateTime ToLocal(string timeZoneId, Instant instant)
    {
        var timeZoneClockService = GetTimeZoneClockService(timeZoneId);
        return timeZoneClockService.ToLocal(instant);
    }

    public List<string> GetTimeZones()
    {
        return timeZoneClockServices?.Select(tz => tz.TimeZone.Id)?.ToList() ?? [];
    }

    private TimeZoneClockService GetTimeZoneClockService(string timeZoneId)
    {
        ArgumentNullException.ThrowIfNull(timeZoneClockServices);

        var timeZoneClockService = timeZoneClockServices.FirstOrDefault(tz => tz.TimeZone.Id == timeZoneId);
        if (timeZoneClockService == null)
            throw new ArgumentException($"Time zone with ID '{timeZoneId}' not found for country '{country}'.");

        return timeZoneClockService;
    }
}