namespace SyncService.Helpers.CompareHelpers;

public class PropertyRange
{
    public double Min { get; }
    public double Max { get; }

    public PropertyRange(double min, double max)
    {
        if (max < min) throw new ArgumentException("Max не может быть меньше Min.");
        Min = min;
        Max = max;
    }
}