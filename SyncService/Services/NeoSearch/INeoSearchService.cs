using SyncService.DTOs.NeoSearch;

namespace SyncService.Services.NeoSearch;

public interface INeoSearchService
{
    Task<IEnumerable<NeoSearchResult>> SearchAsync(NeoSearchRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<NeoSearchResult>?> FindSimilarAsync(string neoId, CancellationToken cancellationToken);
}