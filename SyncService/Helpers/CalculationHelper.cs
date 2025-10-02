using System.Numerics;

namespace SyncService.Helpers;

public static class CalculationHelper
{
    public static T GetAverage<T>(params T[] values) where T : INumber<T>
    {
        var defaultValue = default(T);
        if (values.Length == 0) return defaultValue;
        
        var sum = defaultValue;
        var count = defaultValue;

        foreach (var number in values)
        {
            sum += number;
            count++;
        }
        return sum / count;
    }

    public static T GetMedian<T>(IEnumerable<T> values) where T : INumber<T>
    {
        var sorted = values.OrderBy(x => x).ToList();
        if (sorted.Count == 0) return default;
        
        var mid = sorted.Count / 2;
        
        return mid % 2 == 1 
            ? sorted[mid] 
            : (sorted[mid - 1] + sorted[mid]) / T.CreateTruncating(2);
    }
    
    public static double GetStandardDeviation<T>(IEnumerable<T> values) where T : INumber<T>
    {
        var list = values.ToList();
        if (list.Count == 0) return 0;

        var mean = list.Average(double.CreateChecked);

        var variance = list.Average(v =>
        {
            var val = double.CreateChecked(v);
            var diff = val - mean;
            return diff * diff;
        });

        return Math.Sqrt(variance);
    }

    public static T CalculateChangeRatio<T>(T newValue, T oldValue) where T : INumber<T>
    {
        if (T.IsZero(newValue) && T.IsZero(oldValue))
            return T.Zero;
        if (T.IsZero(oldValue))
            throw new ArgumentException($"{nameof(oldValue)} should not be zero", nameof(oldValue));
        
        return (newValue - oldValue) / oldValue;
    }
    
    public static T CalculateChangeRatioWithInfinite<T>(T newValue, T oldValue, T infiniteValue) where T : INumber<T>
    {
        return T.IsZero(oldValue) 
            ? infiniteValue 
            : CalculateChangeRatio(newValue, oldValue);
    }
}