using SyncService.NeoApiComponents.Main;

namespace SyncService.NasaApi.Client;

public interface INasaApiClient
{
    Task<NeoResponse?> GetNeoResponseByApi(CancellationToken cancellationToken);
}