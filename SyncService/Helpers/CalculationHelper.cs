using System.Numerics;

namespace SyncService.Helpers;

public static class CalculationHelper
{
    public static T GetAverage<T>(params T[] values) where T : INumber<T>
    {
        var sum = default(T);
        var count = default(T);

        foreach (var number in values)
        {
            sum += number;
            count++;
        }
        return sum / count;
    }

    public static double CalculateChangeRatio(double newValue, double oldValue) => (newValue - oldValue) / oldValue;
}