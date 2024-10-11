using System.Text.Json;

namespace BitwiseMind.Globalization.Tests;

public class HolidayTypesJsonConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new HolidayTypesJsonConverter() }
    };

    [Fact]
    public void Read_ShouldDeserializeValidJsonArrayToHolidayTypesArray()
    {
        // Arrange
        var json = "[\"Public\", \"Bank\", \"School\"]";

        // Act
        var result = JsonSerializer.Deserialize<HolidayTypes[]>(json, _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Length);
        Assert.Contains(HolidayTypes.Public, result);
        Assert.Contains(HolidayTypes.Bank, result);
        Assert.Contains(HolidayTypes.School, result);
    }

    [Fact]
    public void Read_ShouldReturnEmptyArrayForEmptyJsonArray()
    {
        // Arrange
        var json = "[]";

        // Act
        var result = JsonSerializer.Deserialize<HolidayTypes[]>(json, _options);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Read_ShouldIgnoreInvalidHolidayTypes()
    {
        // Arrange
        var json = "[\"Public\", \"InvalidType\", \"School\"]";

        // Act
        var result = JsonSerializer.Deserialize<HolidayTypes[]>(json, _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Length);
        Assert.Contains(HolidayTypes.Public, result);
        Assert.Contains(HolidayTypes.School, result);
    }

    [Fact]
    public void Write_ShouldSerializeHolidayTypesArrayToJsonArray()
    {
        // Arrange
        var holidayTypes = new[] { HolidayTypes.Public, HolidayTypes.Bank, HolidayTypes.School };

        // Act
        var json = JsonSerializer.Serialize(holidayTypes, _options);

        // Assert
        Assert.Equal("[\"Public\",\"Bank\",\"School\"]", json);
    }

    [Fact]
    public void Write_ShouldSerializeEmptyHolidayTypesArrayToEmptyJsonArray()
    {
        // Arrange
        var holidayTypes = Array.Empty<HolidayTypes>();

        // Act
        var json = JsonSerializer.Serialize(holidayTypes, _options);

        // Assert
        Assert.Equal("[]", json);
    }
}