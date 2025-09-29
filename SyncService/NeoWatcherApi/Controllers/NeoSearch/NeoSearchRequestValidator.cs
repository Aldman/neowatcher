using SyncService.Constants;
using SyncService.DTOs.NeoSearch;

namespace SyncService.NeoWatcherApi.Controllers.NeoSearch;

public static class NeoSearchRequestValidator
{
    public static void Validate(NeoSearchRequest request)
    {
        if (request.Page < 1)
            throw new ArgumentException("Page must be greater than 0.");
        
        if (request is { MaxDiameter: not null, MinDiameter: not null }
            && request.MaxDiameter <= request.MinDiameter)
            throw new ArgumentException(message: CommonExceptionTexts.MaxDiameterLessThenMin);
        
        if (request.MaxDiameter is <= 0)
            throw new ArgumentException(message: CommonExceptionTexts.MaxDiameterLessThenZero);
        
        if (request.MinDiameter is < 0)
            throw new ArgumentException(message: CommonExceptionTexts.MinDiameterNegative);
    }
}