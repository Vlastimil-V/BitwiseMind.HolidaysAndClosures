namespace BitwiseMind.Globalization;

internal class ClosedDaysManager(IAsyncEnumerable<PublicHoliday> holidays) : HolidayManager(holidays), IClosedDaysManager
{
    public bool IsClosed(DateOnly date) =>
        date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || IsHoliday(date);

    public bool IsClosedToday() =>
        IsClosed(DateOnly.FromDateTime(DateTime.Now));
}