using SyncService.BackgroundLogic;
using SyncService.EfComponents;

namespace SyncService.Extensions;

public static class IServiceCollectionExtensions
{
    public static void Initialize(this IServiceCollection services)
    {
        services.AddDbContext<NeoContext>();
        services.AddScoped<HttpClient>();
        services.AddScoped<INasaApiClient, NasaApiClient>();
        services.AddScoped<SyncJob>();
        services.AddHostedService<SyncServiceWorker>();
    }
}