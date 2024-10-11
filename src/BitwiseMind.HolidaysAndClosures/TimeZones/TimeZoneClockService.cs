using NodaTime;
using NodaTime.TimeZones;

namespace BitwiseMind.Globalization.TimeZones;

public class TimeZoneClockService : IClockService
{
    public TimeZoneClockService(DateTimeZone? timezone, LocalDateTime? offsetDateTime)
        : this(timezone, offsetDateTime, SystemClock.Instance) { }

    protected TimeZoneClockService(DateTimeZone? timezone, LocalDateTime? offsetDateTime, IClock clock)
    {
        Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        OffsetDateTime = offsetDateTime;
        TimeZone = timezone ?? throw new ArgumentNullException(nameof(timezone));
    }

    protected IClock Clock { get; }
    protected LocalDateTime? OffsetDateTime { get; }

    public Instant Now
    {
        get
        {
            if (OffsetDateTime == null)
                return Clock.GetCurrentInstant();

            var offsetLocalTime = new LocalDateTime(OffsetDateTime.Value.Year, OffsetDateTime.Value.Month, OffsetDateTime.Value.Day, OffsetDateTime.Value.Hour, OffsetDateTime.Value.Minute, OffsetDateTime.Value.Second);
            return offsetLocalTime.InZoneLeniently(TimeZone!).ToInstant();
        }
    }

    public DateTimeZone TimeZone { get; init; }
    public DateTimeOffset CurrentTime => Now.ToDateTimeOffset();

    protected internal LocalDateTime LocalNow => Now.InZone(TimeZone!).LocalDateTime;

    protected internal Instant ToInstant(LocalDateTime local) =>
        local.InZone(TimeZone!, Resolvers.LenientResolver).ToInstant();

    protected internal LocalDateTime ToLocal(Instant instant) =>
        instant.InZone(TimeZone!).LocalDateTime;
}