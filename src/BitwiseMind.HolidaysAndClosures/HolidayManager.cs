namespace BitwiseMind.Globalization;

internal class HolidayManager(IAsyncEnumerable<PublicHoliday> holidays) : IHolidayManager
{
    public HashSet<PublicHoliday> Holidays { get; init; } = holidays.ToHashSetAsync().SyncResult();

    public bool IsHoliday(DateOnly date)
    {
        return Holidays.Any(holiday =>
        {
            var (start, end) = holiday.OccurrenceDetails;
            return date >= start && date <= end;
        });
    }

    public bool IsHolidayToday()
        => IsHoliday(DateOnly.FromDateTime(DateTime.Now));
}