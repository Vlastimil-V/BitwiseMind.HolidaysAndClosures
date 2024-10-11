using System.Text.Json.Serialization;

namespace BitwiseMind.Globalization;

internal record HolidayDescriptor
{
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    [JsonPropertyName("localName")]
    public string? LocalName { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("fixed")]
    public bool? Fixed { get; set; }

    [JsonPropertyName("global")]
    public bool? Global { get; set; }

    [JsonPropertyName("counties")]
    public string[]? Counties { get; set; }

    [JsonPropertyName("launchYear")]
    public int? LaunchYear { get; set; }

    [JsonConverter(typeof(HolidayTypesJsonConverter))]
    [JsonPropertyName("types")]
    public HolidayTypes[]? Types { get; set; }
}