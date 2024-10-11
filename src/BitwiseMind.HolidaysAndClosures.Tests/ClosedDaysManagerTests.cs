using Moq;

namespace BitwiseMind.Globalization.Tests;

public class ClosedDaysManagerTests
{
    [Fact]
    public void IsClosed_WhenSaturdayOrSunday_ReturnsTrue()
    {
        var publicHolidays = new HashSet<PublicHoliday>().ToAsyncEnumerable().GetAsyncEnumerator(CancellationToken.None);

        // Arrange
        var mockHolidays = new Mock<IAsyncEnumerable<PublicHoliday>>();
        mockHolidays.Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(publicHolidays);
        var closedDaysManager = new ClosedDaysManager(mockHolidays.Object);

        // Act
        var isClosedSaturday = closedDaysManager.IsClosed(new DateOnly(2024, 10, 12)); // Saturday
        var isClosedSunday = closedDaysManager.IsClosed(new DateOnly(2024, 10, 13)); // Sunday

        // Assert
        Assert.True(isClosedSaturday);
        Assert.True(isClosedSunday);
    }

    [Fact]
    public void IsClosed_WhenHoliday_ReturnsTrue()
    {
        // Arrange
        var publicHoliday = new PublicHoliday("Test Holiday", "US", (new DateOnly(2024, 12, 25), new DateOnly(2024, 12, 25)), Mock.Of<IPublicHolidayProvider>());
        var holidays = new HashSet<PublicHoliday> { publicHoliday }.ToAsyncEnumerable();
        var closedDaysManager = new ClosedDaysManager(holidays);

        // Act
        var isClosed = closedDaysManager.IsClosed(new DateOnly(2024, 12, 25));

        // Assert
        Assert.True(isClosed);
    }

    [Fact]
    public void IsClosed_WhenWeekdayAndNotHoliday_ReturnsFalse()
    {
        var publicHolidays = new HashSet<PublicHoliday>().ToAsyncEnumerable().GetAsyncEnumerator(CancellationToken.None);

        // Arrange
        var mockHolidays = new Mock<IAsyncEnumerable<PublicHoliday>>();
        mockHolidays.Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(publicHolidays);
        var closedDaysManager = new ClosedDaysManager(mockHolidays.Object);

        // Act
        var isClosed = closedDaysManager.IsClosed(new DateOnly(2024, 10, 9)); // Wednesday

        // Assert
        Assert.False(isClosed);
    }

    [Fact]
    public void IsClosedToday_ReturnsExpectedResult()
    {
        var publicHolidays = new HashSet<PublicHoliday>().ToAsyncEnumerable().GetAsyncEnumerator(CancellationToken.None);

        // Arrange
        var mockHolidays = new Mock<IAsyncEnumerable<PublicHoliday>>();
        mockHolidays.Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(publicHolidays);

        var closedDaysManager = new ClosedDaysManager(mockHolidays.Object);
        var today = DateOnly.FromDateTime(DateTime.Now);
        var expectedResult = today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday;

        // Act
        var isClosedToday = closedDaysManager.IsClosedToday();

        // Assert
        Assert.Equal(expectedResult, isClosedToday);
    }
}