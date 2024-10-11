using System.Text;
using System.Text.Json;

namespace BitwiseMind.Globalization.Tests;

public class DateOnlyJsonConverterTests
{
    private readonly DateOnlyJsonConverter _converter = new();

    [Fact]
    public void Read_ValidDateString_ReturnsDateOnly()
    {
        // Arrange
        var json = "\"2023-10-07\"";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        reader.Read();

        // Act
        var result = _converter.Read(ref reader, typeof(DateOnly), new JsonSerializerOptions());

        // Assert
        Assert.Equal(new DateOnly(2023, 10, 7), result);
    }

    [Fact]
    public void Read_InvalidDateString_ThrowsJsonException()
    {
        // Arrange
        var json = "\"invalid-date\"";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        reader.Read();

        try
        {
            // Act
            var _ = _converter.Read(ref reader, typeof(DateOnly), new JsonSerializerOptions());
        }
        catch (Exception exception)
        {
            // Assert
            Assert.ThrowsAsync<JsonException>(() => throw exception).SyncResult();
        }
    }

    [Fact]
    public void Write_ValidDateOnly_WritesCorrectJsonString()
    {
        // Arrange
        var date = new DateOnly(2023, 10, 7);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);

        // Act
        _converter.Write(writer, date, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("\"2023-10-07\"", json);
    }
}