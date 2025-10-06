using SyncService.DTOs.NeoSearch;

namespace SyncService.Services.NeoSearch;

public interface INeoSearchService
{
    Task<IEnumerable<NeoSearchResult>> SearchAsync(NeoSearchRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<NeoSearchResult>?> FindSimilarAsync(string neoId, CancellationToken cancellationToken);
    Task<IEnumerable<string>?> GetSuggestionsAsync(string query, int limit = 10, CancellationToken cancellationToken = default);
}