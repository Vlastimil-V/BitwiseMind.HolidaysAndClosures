namespace BitwiseMind.Globalization.TimeZones;

public static class TimeZoneMappingProvider
{
    private static readonly Lazy<TimeZoneMapping> _timeZoneMapping = new(InitializeMapping);

    public static TimeZoneMapping GetCountryTimeZones() => _timeZoneMapping.Value;

    private static TimeZoneMapping InitializeMapping()
    {
        var countries = new Dictionary<string, Country>
        {
            { "AD", new Country("Andorra", "AD", ["Europe/Andorra"]) },
            { "AL", new Country("Albania", "AL", ["Europe/Tirane"]) },
            { "AT", new Country("Austria", "AT", ["Europe/Vienna"]) },
            { "BA", new Country("Bosnia & Herzegovina", "BA", ["Europe/Belgrade"]) },
            { "BE", new Country("Belgium", "BE", ["Europe/Brussels"]) },
            { "BG", new Country("Bulgaria", "BG", ["Europe/Sofia"]) },
            { "BY", new Country("Belarus", "BY", ["Europe/Minsk"]) },
            { "CH", new Country("Switzerland", "CH", ["Europe/Zurich"]) },
            { "CY", new Country("Cyprus", "CY", ["Asia/Nicosia"]) },
            { "CZ", new Country("Czech Republic", "CZ", ["Europe/Prague"]) },
            { "DE", new Country("Germany", "DE", ["Europe/Berlin"]) },
            { "DK", new Country("Denmark", "DK", ["Europe/Copenhagen"]) },
            { "EE", new Country("Estonia", "EE", ["Europe/Tallinn"]) },
            { "ES", new Country("Spain", "ES", ["Europe/Madrid", "Atlantic/Canary"]) },
            { "FI", new Country("Finland", "FI", ["Europe/Helsinki"]) },
            { "FR", new Country("France", "FR", ["Europe/Paris"]) },
            { "GB", new Country("United Kingdom", "GB", ["Europe/London"]) },
            { "GR", new Country("Greece", "GR", ["Europe/Athens"]) },
            { "HR", new Country("Croatia", "HR", ["Europe/Belgrade"]) },
            { "HU", new Country("Hungary", "HU", ["Europe/Budapest"]) },
            { "IE", new Country("Ireland", "IE", ["Europe/Dublin"]) },
            { "IS", new Country("Iceland", "IS", ["Atlantic/Reykjavik"]) },
            { "IT", new Country("Italy", "IT", ["Europe/Rome"]) },
            { "LI", new Country("Liechtenstein", "LI", ["Europe/Zurich"]) },
            { "LT", new Country("Lithuania", "LT", ["Europe/Vilnius"]) },
            { "LU", new Country("Luxembourg", "LU", ["Europe/Luxembourg"]) },
            { "LV", new Country("Latvia", "LV", ["Europe/Riga"]) },
            { "MC", new Country("Monaco", "MC", ["Europe/Monaco"]) },
            { "MD", new Country("Moldova", "MD", ["Europe/Chisinau"]) },
            { "ME", new Country("Montenegro", "ME", ["Europe/Belgrade"]) },
            { "MK", new Country("North Macedonia", "MK", ["Europe/Belgrade"]) },
            { "MT", new Country("Malta", "MT", ["Europe/Malta"]) },
            { "NL", new Country("Netherlands", "NL", ["Europe/Amsterdam"]) },
            { "NO", new Country("Norway", "NO", ["Europe/Oslo"]) },
            { "PL", new Country("Poland", "PL", ["Europe/Warsaw"]) },
            { "PT", new Country("Portugal", "PT", ["Europe/Lisbon", "Atlantic/Madeira", "Atlantic/Azores"]) },
            { "RO", new Country("Romania", "RO", ["Europe/Bucharest"]) },
            { "RS", new Country("Serbia", "RS", ["Europe/Belgrade"]) },
            { "SE", new Country("Sweden", "SE", ["Europe/Stockholm"]) },
            { "SI", new Country("Slovenia", "SI", ["Europe/Belgrade"]) },
            { "SK", new Country("Slovakia", "SK", ["Europe/Prague"]) },
            { "SM", new Country("San Marino", "SM", ["Europe/Rome"]) },
            { "UA", new Country("Ukraine", "UA", ["Europe/Kiev", "Europe/Uzhgorod", "Europe/Zaporozhye"]) },
            { "VA", new Country("Vatican City", "VA", ["Europe/Rome"]) }
        };

        var zones = new Dictionary<string, TimeZone>
        {
            { "Europe/Andorra", new TimeZone("Europe/Andorra", 42.5, 1.5, ["AD"], "Andorra standard time") },
            { "Europe/Tirane", new TimeZone("Europe/Tirane", 41.3275, 19.8189, ["AL"], "Albania standard time") },
            { "Europe/Vienna", new TimeZone("Europe/Vienna", 48.2082, 16.3738, ["AT"], "Austria standard time") },
            { "Europe/Belgrade", new TimeZone("Europe/Belgrade", 44.7866, 20.4489, ["BA", "HR", "ME", "MK", "RS", "SI"], "Belgrade standard time") },
            { "Europe/Brussels", new TimeZone("Europe/Brussels", 50.8503, 4.3517, ["BE"], "Belgium standard time") },
            { "Europe/Sofia", new TimeZone("Europe/Sofia", 42.6977, 23.3219, ["BG"], "Bulgaria standard time") },
            { "Europe/Minsk", new TimeZone("Europe/Minsk", 53.9006, 27.5590, ["BY"], "Belarus standard time") },
            { "Europe/Zurich", new TimeZone("Europe/Zurich", 47.3769, 8.5417, ["CH", "LI"], "Zurich standard time") },
            { "Asia/Nicosia", new TimeZone("Asia/Nicosia", 35.1856, 33.3823, ["CY"], "Cyprus standard time") },
            { "Europe/Prague", new TimeZone("Europe/Prague", 50.0755, 14.4378, ["CZ", "SK"], "Czech Republic standard time") },
            { "Europe/Berlin", new TimeZone("Europe/Berlin", 52.5200, 13.4050, ["DE"], "Germany standard time") },
            { "Europe/Copenhagen", new TimeZone("Europe/Copenhagen", 55.6761, 12.5683, ["DK"], "Denmark standard time") },
            { "Europe/Tallinn", new TimeZone("Europe/Tallinn", 59.4370, 24.7536, ["EE"], "Estonia standard time") },
            { "Europe/Madrid", new TimeZone("Europe/Madrid", 40.4168, -3.7038, ["ES"], "Spain standard time") },
            { "Atlantic/Canary", new TimeZone("Atlantic/Canary", 28.2916, -16.6291, ["ES"], "Canary Islands standard time") },
            { "Europe/Helsinki", new TimeZone("Europe/Helsinki", 60.1699, 24.9384, ["FI"], "Finland standard time") },
            { "Europe/Paris", new TimeZone("Europe/Paris", 48.8566, 2.3522, ["FR"], "France standard time") },
            { "Europe/London", new TimeZone("Europe/London", 51.5074, -0.1278, ["GB"], "United Kingdom standard time") },
            { "Europe/Athens", new TimeZone("Europe/Athens", 37.9838, 23.7275, ["GR"], "Greece standard time") },
            { "Europe/Budapest", new TimeZone("Europe/Budapest", 47.4979, 19.0402, ["HU"], "Hungary standard time") },
            { "Europe/Dublin", new TimeZone("Europe/Dublin", 53.3498, -6.2603, ["IE"], "Ireland standard time") },
            { "Atlantic/Reykjavik", new TimeZone("Atlantic/Reykjavik", 64.1355, -21.8954, ["IS"], "Iceland standard time") },
            { "Europe/Rome", new TimeZone("Europe/Rome", 41.9028, 12.4964, ["IT", "SM", "VA"], "Italy standard time") },
            { "Europe/Vilnius", new TimeZone("Europe/Vilnius", 54.6872, 25.2797, ["LT"], "Lithuania standard time") },
            { "Europe/Luxembourg", new TimeZone("Europe/Luxembourg", 49.6117, 6.1319, ["LU"], "Luxembourg standard time") },
            { "Europe/Riga", new TimeZone("Europe/Riga", 56.9496, 24.1052, ["LV"], "Latvia standard time") },
            { "Europe/Monaco", new TimeZone("Europe/Monaco", 43.7374, 7.4208, ["MC"], "Monaco standard time") },
            { "Europe/Chisinau", new TimeZone("Europe/Chisinau", 47.0105, 28.8638, ["MD"], "Moldova standard time") },
            { "Europe/Malta", new TimeZone("Europe/Malta", 35.9375, 14.3754, ["MT"], "Malta standard time") },
            { "Europe/Amsterdam", new TimeZone("Europe/Amsterdam", 52.3676, 4.9041, ["NL"], "Netherlands standard time") },
            { "Europe/Oslo", new TimeZone("Europe/Oslo", 59.9139, 10.7522, ["NO"], "Norway standard time") },
            { "Europe/Warsaw", new TimeZone("Europe/Warsaw", 52.2297, 21.0122, ["PL"], "Poland standard time") },
            { "Europe/Lisbon", new TimeZone("Europe/Lisbon", 38.7223, -9.1393, ["PT"], "Portugal standard time") },
            { "Atlantic/Madeira", new TimeZone("Atlantic/Madeira", 32.7607, -16.9595, ["PT"], "Madeira Islands standard time") },
            { "Atlantic/Azores", new TimeZone("Atlantic/Azores", 37.7412, -25.6756, ["PT"], "Azores Islands standard time") },
            { "Europe/Bucharest", new TimeZone("Europe/Bucharest", 44.4268, 26.1025, ["RO"], "Romania standard time") },
            { "Europe/Stockholm", new TimeZone("Europe/Stockholm", 59.3293, 18.0686, ["SE"], "Sweden standard time") },
            { "Europe/Kiev", new TimeZone("Europe/Kiev", 50.4501, 30.5234, ["UA"], "Ukraine standard time") },
            { "Europe/Uzhgorod", new TimeZone("Europe/Uzhgorod", 48.6238, 22.2967, ["UA"], "Uzhgorod standard time") },
            { "Europe/Zaporozhye", new TimeZone("Europe/Zaporozhye", 47.8388, 35.1396, ["UA"], "Zaporozhye standard time") }
        };

        return new TimeZoneMapping(countries, zones);
    }
}