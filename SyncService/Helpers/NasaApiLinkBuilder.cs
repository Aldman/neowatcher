using System.Text;

namespace SyncService.Helpers;

public static class NasaApiLinkBuilder
{
    private const string DateFormat = "yyyy-MM-dd";
    private const string ApiKey = "TPpa7xjLe7kTZyOv4SuJtSfr8znerFabJ5LBrwJ0";

    public static string GetLinkWithOnlyStartDate(DateTime startDate)
    {
        var asString = startDate.ToString(DateFormat);
        return BuildLink(startDate: asString);
    }
    
    public static string GetLinkWithOnlyEndDate(DateTime endDate)
    {
        var asString = endDate.ToString(DateFormat);
        return BuildLink(startDate: null, endDate: asString);
    }
    
    public static string GetLinkWithDateRange(DateTime startDate, DateTime endDate)
    {
        var endDateString = endDate.ToString(DateFormat);
        var startDateString = startDate.ToString(DateFormat);
        
        return BuildLink(startDate: startDateString, endDate: endDateString);
    }

    private static string BuildLink(string? startDate, string? endDate = null)
    {
        const string mainPart = "https://api.nasa.gov/neo/rest/v1/feed?";

        var builder = new StringBuilder(mainPart);
        if (startDate != null)
        {
            builder.Append($"start_date={startDate}&");
        }

        if (endDate != null)
        {
            builder.Append($"end_date={endDate}&");
        }
        
        builder.Append($"api_key={ApiKey}");
        return builder.ToString();
    }
}