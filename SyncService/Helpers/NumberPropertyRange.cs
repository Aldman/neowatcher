using System.Numerics;

namespace SyncService.Helpers;

public class NumberPropertyRange <T> where T : INumber<T>
{
    public T Min { get; }
    public T Max { get; }
    
    public NumberPropertyRange(T min, T max)
    {
        if (max < min) throw new ArgumentException("The Max cannot be less than the Min.");
        
        Min = min;
        Max = max;
    }
    
    public T Diff => Max - Min;
    
    public bool DiffIsZero => Diff == T.Zero;
}