using SyncService.EfComponents.Repository;
using SyncService.Helpers;
using SyncService.NasaApi.Client;

namespace SyncService.BackgroundLogic;

public class SyncJob
{
    private readonly INasaApiClient _apiClient;
    private readonly INeoRepository _neoRepository;

    public SyncJob(INasaApiClient apiClient, INeoRepository neoRepository)
    {
        _apiClient = apiClient;
        _neoRepository = neoRepository;
    }

    public async Task UpdateDataOfNasaAsync(CancellationToken cancellationToken = default)
    {
        var neoResponse = await _apiClient.GetNeoResponseByApi(cancellationToken);

        var dto = ApiToDbSetsDataMapper.MapAndLink(neoResponse);
        // todo:
        // await _neoRepository.SaveOrUpdate(dto, cancellationToken);
    }
}