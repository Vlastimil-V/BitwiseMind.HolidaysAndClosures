namespace BitwiseMind.Globalization;

internal static class TaskExtensions
{
    public static T SyncResult<T>(this Task<T> task) => task.GetAwaiter().GetResult();
    public static T SyncResult<T>(this ValueTask<T> task) => task.GetAwaiter().GetResult();
}