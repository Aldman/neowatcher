namespace SyncService.Extensions;

public static class DoubleExtensions
{
    public static string ToPercentage(this double value) => $"{value:P}";
}