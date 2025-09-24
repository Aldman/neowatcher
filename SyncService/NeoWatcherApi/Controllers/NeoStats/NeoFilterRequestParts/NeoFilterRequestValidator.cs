using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.Constants;

namespace SyncService.NeoWatcherApi.Controllers.NeoStats.NeoFilterRequestParts;

public static class NeoFilterRequestValidator
{
    public static void Validate(NeoFilterRequest request)
    {
        if (request is { From: not null, To: not null }
            && request.From >= request.To)
            throw new ArgumentException(message: CommonExceptionTexts.FromMoreThanTo);
        
        if (request.MaxDiameter is <= 0)
            throw new ArgumentException("MaxDiameter must be greater than 0");
        
        if (request.MinDiameter is < 0)
            throw new ArgumentException("MinDiameter should not be negative ");
    }
}