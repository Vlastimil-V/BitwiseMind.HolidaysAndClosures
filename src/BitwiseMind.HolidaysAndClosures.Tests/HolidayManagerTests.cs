using Moq;

namespace BitwiseMind.Globalization.Tests;

public class HolidayManagerTests
{
    [Fact]
    public void IsHoliday_WhenDateIsHoliday_ReturnsTrue()
    {
        // Arrange
        var publicHoliday = new PublicHoliday("Test Holiday", "US", (new DateOnly(2024, 12, 25), new DateOnly(2024, 12, 25)), Mock.Of<IPublicHolidayProvider>());
        var holidays = new HashSet<PublicHoliday> { publicHoliday }.ToAsyncEnumerable();
        var holidayManager = new HolidayManager(holidays);

        // Act
        var isHoliday = holidayManager.IsHoliday(new DateOnly(2024, 12, 25));

        // Assert
        Assert.True(isHoliday);
    }

    [Fact]
    public void IsHoliday_WhenDateIsNotHoliday_ReturnsFalse()
    {
        // Arrange
        var publicHoliday = new PublicHoliday("Test Holiday", "US", (new DateOnly(2024, 12, 25), new DateOnly(2024, 12, 25)), Mock.Of<IPublicHolidayProvider>());
        var holidays = new HashSet<PublicHoliday> { publicHoliday }.ToAsyncEnumerable();
        var holidayManager = new HolidayManager(holidays);

        // Act
        var isHoliday = holidayManager.IsHoliday(new DateOnly(2024, 10, 9));

        // Assert
        Assert.False(isHoliday);
    }

    [Fact]
    public void IsHolidayToday_ReturnsExpectedResult()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);
        var publicHoliday = new PublicHoliday("Test Holiday", "US", (today, today), Mock.Of<IPublicHolidayProvider>());
        var holidays = new HashSet<PublicHoliday> { publicHoliday }.ToAsyncEnumerable();
        var holidayManager = new HolidayManager(holidays);

        // Act
        var isHolidayToday = holidayManager.IsHolidayToday();

        // Assert
        Assert.True(isHolidayToday);
    }
}