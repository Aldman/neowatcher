using SyncService.NeoApiComponents.Main;

namespace SyncService;

public interface INasaApiClient
{
    Task<NeoResponse?> GetNeoResponseByApi(CancellationToken cancellationToken);
}