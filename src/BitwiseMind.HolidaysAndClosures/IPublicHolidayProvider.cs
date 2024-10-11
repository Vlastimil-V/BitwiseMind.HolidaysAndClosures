namespace BitwiseMind.Globalization;

public interface IPublicHolidayProvider
{
    IAsyncEnumerable<PublicHoliday> GetPublicHolidaysAsync(int year, string countryCode, CancellationToken cancellationToken = default);
    IEnumerable<PublicHoliday> GetPublicHolidays(int year, string countryCode);
}