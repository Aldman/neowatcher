using SyncService.BackgroundLogic;
using SyncService.EfComponents;
using SyncService.EfComponents.Contexts;

namespace SyncService.Extensions;

public static class IServiceCollectionExtensions
{
    public static void Initialize(this IServiceCollection services)
    {
        services.AddDbContext<NeoContext>();
        services.AddScoped<HttpClient>();
        services.AddScoped<SyncJob>();
        services.AddHostedService<SyncServiceWorker>();
    }
}