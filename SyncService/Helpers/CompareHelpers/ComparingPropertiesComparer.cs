namespace SyncService.Helpers.CompareHelpers;

public static class ComparingPropertiesComparer
{
    public static double CalculateSimilarity(ComparingProperties obj1, ComparingProperties obj2)
    {
        var diameterMinDiff = obj1.DiameterMin - obj2.DiameterMin;
        var diameterMaxDiff = obj1.DiameterMax - obj2.DiameterMax;
        var velocityDiff = obj1.Velocity - obj2.Velocity;
        
        var distance = Math.Sqrt(diameterMinDiff * diameterMinDiff 
                                 + diameterMaxDiff * diameterMaxDiff 
                                 + velocityDiff * velocityDiff);
        
        return 1 / (1 + distance);
    }
}