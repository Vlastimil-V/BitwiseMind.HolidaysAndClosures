namespace BitwiseMind.Globalization;

public record PublicHoliday : RecurringHoliday<PublicHoliday>
{
    private readonly IPublicHolidayProvider _holidayClient;

    public PublicHoliday(string? name, string? countryCode, (DateOnly Start, DateOnly End) occurrenceDetails, IPublicHolidayProvider? holidayClient)
        : base(name, countryCode)
    {
        ArgumentNullException.ThrowIfNull(holidayClient);
        _holidayClient = holidayClient;

        ArgumentNullException.ThrowIfNull(occurrenceDetails);
        OccurrenceDetails = occurrenceDetails;

        var (start, end) = occurrenceDetails;
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(end);
    }

    public override PublicHoliday GetNextOccurrence() =>
        GetNextOccurrenceAsync().SyncResult();

    public override async Task<PublicHoliday> GetNextOccurrenceAsync(CancellationToken cancellationToken = default)
    {
        var (start, _) = OccurrenceDetails;
        var nextYear = start.Year + 1;

        await foreach (var holiday in _holidayClient.GetPublicHolidaysAsync(nextYear, CountryCode).WithCancellation(cancellationToken))
        {
            if (holiday.Name == Name && holiday.OccurrenceDetails.Start >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return holiday;
            }
        }

        throw new InvalidOperationException($"No data for holiday {Name} has been found.");
    }

    public sealed override (DateOnly Start, DateOnly End) OccurrenceDetails { get; protected set; } = default;

    public void Deconstruct(out DateOnly start, out DateOnly end) =>
        (start, end) = OccurrenceDetails;
}