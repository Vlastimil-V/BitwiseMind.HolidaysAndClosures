namespace BitwiseMind.Globalization;

public interface IHolidayManager
{
    bool IsHoliday(DateOnly date);
    bool IsHolidayToday();
    HashSet<PublicHoliday> Holidays { get; }
}