using System.Text.Json.Serialization;

namespace BitwiseMind.Globalization.TimeZones;
public record TimeZone(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("long")] double Long,
    [property: JsonPropertyName("countries")] List<string> Countries,
    [property: JsonPropertyName("comments")] string Comments
);