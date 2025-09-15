using System.ComponentModel;
using System.Text.Json.Serialization;
using SyncService.Api.NeoWatcher.Swagger;
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
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SchemaFilter<EnumSchemaFilter>();
        });
    }
}