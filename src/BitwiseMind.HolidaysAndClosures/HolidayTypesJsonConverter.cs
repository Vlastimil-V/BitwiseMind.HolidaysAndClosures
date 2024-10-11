using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitwiseMind.Globalization;

internal class HolidayTypesJsonConverter : JsonConverter<HolidayTypes[]>
{
    public override HolidayTypes[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var types = new List<HolidayTypes>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (Enum.TryParse(reader.GetString(), out HolidayTypes holidayType))
            {
                types.Add(holidayType);
            }
        }
        return types.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, HolidayTypes[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var type in value)
        {
            writer.WriteStringValue(type.ToString());
        }
        writer.WriteEndArray();
    }
}