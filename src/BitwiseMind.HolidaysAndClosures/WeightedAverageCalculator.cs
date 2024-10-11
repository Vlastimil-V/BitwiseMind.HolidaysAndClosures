namespace BitwiseMind.Globalization;

internal static class WeightedAverageCalculator
{
    public static TimeSpan CalculateWeightedAverage(List<(TimeSpan Period, double Weight)> timeSpansWithWeights)
    {
        if (timeSpansWithWeights == null || timeSpansWithWeights.Count == 0)
        {
            throw new ArgumentException("TimeSpan list must have a non-zero length.");
        }

        var totalWeight = timeSpansWithWeights.Sum(item => item.Weight);
        if (totalWeight == 0)
        {
            throw new ArgumentException("Total weight must not be zero.");
        }

        var weightedTicksSum = timeSpansWithWeights.Sum(item => item.Period.Ticks * item.Weight);
        var weightedAverageTicks = (long)(weightedTicksSum / totalWeight);

        return TimeSpan.FromTicks(weightedAverageTicks);
    }
}