namespace BitwiseMind.Globalization;

public interface IClosedDaysManager: IHolidayManager
{
    bool IsClosed(DateOnly date);
    bool IsClosedToday();
}