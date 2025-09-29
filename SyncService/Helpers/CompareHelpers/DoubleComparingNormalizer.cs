using SyncService.EfComponents.DbSets;

namespace SyncService.Helpers.CompareHelpers;

public class DoubleComparingNormalizer
{
    private readonly PropertyRange _diameter;
    private readonly PropertyRange _velocity;

    public DoubleComparingNormalizer(PropertyRange diameter, PropertyRange velocity)
    {
        _diameter = diameter;
        _velocity = velocity;
    }
    
    public DoubleComparingNormalizer(double minDiameter, double maxDiameter, double minVelocity, double maxVelocity)
    {
        _diameter = new PropertyRange(minDiameter, maxDiameter);
        _velocity = new PropertyRange(minVelocity, maxVelocity);
    }

    private static double NormalizeValue(double value, PropertyRange range)
    {
        // Избегаем деления на ноль, если все значения одинаковы
        if (range.Max - range.Min == 0)
        {
            return 0; 
        }
        return (value - range.Min) / (range.Max - range.Min);
    }

    public ComparingProperties NormalizeForComparing(DbNearEarthObject obj)
    {
        var minDiameter = NormalizeValue(obj.EstimatedDiameterMin, _diameter);
        var maxDiameter = NormalizeValue(obj.EstimatedDiameterMax, _diameter);
        var velocity = NormalizeValue(obj.CloseApproachData.RelativeVelocityKmh, _velocity);

        return new ComparingProperties(maxDiameter, minDiameter, velocity);
    }
}