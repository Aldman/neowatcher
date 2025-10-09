using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SyncService.Api.NeoWatcher.Swagger;
using SyncService.BackgroundLogic;
using SyncService.Constants;
using SyncService.EfComponents;
using SyncService.EfComponents.Repository;
using SyncService.NasaApi.Client;
using SyncService.Services.NeoAnalytics;
using SyncService.Services.NeoReporting;
using SyncService.Services.NeoSearch;
using SyncService.Services.NeoStatistics;
using SyncService.Services.NeoStats;

namespace SyncService.Extensions;

public static class IServiceCollectionExtensions
{
    public static void Initialize(this IServiceCollection services)
    {
        services.AddDbContext<NeoContext>(opts =>
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString(WellKnownNames.DefaultConnection);
        
            opts.UseNpgsql(connectionString);
        });
        services.AddScoped<HttpClient>();
        services.AddScoped<INasaApiClient, NasaApiClient>();
        services.AddScoped<INeoRepository, NeoRepository>();
        services.AddScoped<INeoStatsService, NeoStatsService>();
        services.AddScoped<INeoAnalyticsService, NeoAnalyticsService>();
        services.AddScoped<INeoSearchService, NeoSearchService>();
        services.AddScoped<INeoStatisticsService, NeoStatisticsService>();
        services.AddScoped<INeoReportingService, NeoReportingService>();
        services.AddScoped<SyncJob>();
        services.AddHostedService<SyncServiceWorker>();
        services.AddEndpointsApiExplorer();
        services.AddMemoryCache();
        
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(
                AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);

            options.SchemaFilter<EnumSchemaFilter>();
            options.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
        });
    }
}