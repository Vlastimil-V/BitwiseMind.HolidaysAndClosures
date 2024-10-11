namespace BitwiseMind.Globalization;

internal static class HashSetExtensions
{
    public static void ForEach<T>(this HashSet<T> hashSet, Action<T> action)
    {
        foreach (var item in hashSet)
        {
            action(item);
        }
    }
}