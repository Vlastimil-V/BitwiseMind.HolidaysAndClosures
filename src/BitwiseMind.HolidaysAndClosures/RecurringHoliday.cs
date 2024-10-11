namespace BitwiseMind.Globalization;

public abstract record RecurringHoliday<T> where T: RecurringHoliday<T>
{
    protected RecurringHoliday(string? name, string? countryCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(countryCode);

        Name = name;
        CountryCode = countryCode;
    }

    public string Name { get; }
    public string CountryCode { get; }
    public abstract T GetNextOccurrence();
    public abstract Task<T> GetNextOccurrenceAsync(CancellationToken cancellationToken = default);
    public virtual (DateOnly Start, DateOnly End) OccurrenceDetails { get; protected set; }
    public override string ToString() => $"Holiday: {Name}, Country: {CountryCode}, Occurrence: {OccurrenceDetails.Start} - {OccurrenceDetails.End}";
}