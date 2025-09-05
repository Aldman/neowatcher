using SyncService.Extensions;
using SyncService.Helpers;
using SyncService.NeoApiComponents.Main;

namespace SyncService;

public class NasaApiClient : INasaApiClient
{
    private readonly HttpClient _httpClient;

    public NasaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<NeoResponse?> GetNeoResponseByApi(CancellationToken cancellationToken)
    {
        var apiLink = NasaApiLinkBuilder.GetLinkWithOnlyEndDate(endDate: DateTime.Now);
        var response = await _httpClient.GetAsync(apiLink, cancellationToken);
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        
        return json.ToNeoResponse();
    }
}