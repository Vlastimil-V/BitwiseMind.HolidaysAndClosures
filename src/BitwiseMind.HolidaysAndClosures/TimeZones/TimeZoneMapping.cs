namespace BitwiseMind.Globalization.TimeZones;

public record TimeZoneMapping(
    Dictionary<string, Country> Countries,
    Dictionary<string, TimeZone> TimeZones
)
{
    public List<string> FindTimeZonesByCountryCode(string countryCode) =>
        Countries.TryGetValue(countryCode, out var country) ? country.Zones : new List<string>();
}