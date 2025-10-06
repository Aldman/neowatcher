namespace SyncService.Constants;

public static class CommonExceptionTexts
{
    public const string FromMoreThanTo = "From must be less than To.";
    public const string MaxDiameterLessThenZero = "MaxDiameter must be greater than 0.";
    public const string MinDiameterNegative = "MinDiameter should not be negative.";
    public const string MaxDiameterLessThenMin = "MaxDiameter should be more than MinDiameter.";
}