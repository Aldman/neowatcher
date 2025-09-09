using SyncService.BackgroundLogic;
using SyncService.EfComponents;
using SyncService.EfComponents.Repository;
using SyncService.NasaApi.Client;

namespace SyncService.Extensions;

public static class IServiceCollectionExtensions
{
    public static void Initialize(this IServiceCollection services)
    {
        services.AddDbContext<NeoContext>();
        services.AddScoped<HttpClient>();
        services.AddScoped<INasaApiClient, NasaApiClient>();
        services.AddScoped<INeoRepository, NeoRepository>();
        services.AddScoped<SyncJob>();
        services.AddHostedService<SyncServiceWorker>();
    }
}