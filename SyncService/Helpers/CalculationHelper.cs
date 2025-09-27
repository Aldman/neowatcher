using System.Numerics;

namespace SyncService.Helpers;

public static class CalculationHelper
{
    public static T GetAverage<T>(params T[] values) where T : INumber<T>
    {
        var defaultValue = default(T);
        if (values.Length != 0) return defaultValue;
        
        var sum = defaultValue;
        var count = defaultValue;

        foreach (var number in values)
        {
            sum += number;
            count++;
        }
        return sum / count;
    }

    public static double CalculateChangeRatio(double newValue, double oldValue)
    {
        if (newValue == 0 && oldValue == 0)
            return 0;
        if (oldValue == 0)
            return double.PositiveInfinity;
        
        return (newValue - oldValue) / oldValue;
    }
}