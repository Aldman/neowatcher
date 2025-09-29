using SyncService.DTOs.NeoSearch;

namespace SyncService.Services.NeoSearch;

public class NeoSearchService : INeoSearchService
{
    public Task<IEnumerable<NeoSearchResult>> SearchAsync(
        NeoSearchRequest request, 
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}