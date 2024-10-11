using NodaTime;

namespace BitwiseMind.Globalization.TimeZones;

public interface IClockService
{
    DateTimeOffset CurrentTime { get; }
    DateTimeZone? TimeZone { get; }
}