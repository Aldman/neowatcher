using Microsoft.EntityFrameworkCore;
using Npgsql;
using SyncService.EfComponents;
using SyncService.EfComponents.Repository;
using SyncService.Services.NeoStats;

namespace NeoTesting.Auxiliary;

public class ServiceProviderTemplate : IAsyncLifetime
{
    private PostgresDb? _db;
    public ServiceCollection? Services { get; private set; }

    public async Task<ServiceProvider> GetServiceProviderWithInitializedDbAsync(CancellationToken cancellationToken = default)
    {
        var provider = Services!.BuildServiceProvider();
        var dbContext = provider.GetRequiredService<NeoContext>();

        const string targetMigrationName = "20250916094643_RemoveNotMappedAttribute";
        var migrations = await dbContext.Database.GetAppliedMigrationsAsync(cancellationToken);
        if (migrations.Contains(targetMigrationName)) 
            return provider;
        
        await dbContext.Database.MigrateAsync(targetMigration: targetMigrationName, CancellationToken.None);

        await using var connection = new NpgsqlConnection(_db!.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        var sql = await File.ReadAllTextAsync(Path.Combine("Resources", "testDataBackup.sql"), cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);

        return provider;
    }
    
    public async Task InitializeAsync()
    {
        var services = new ServiceCollection();
        _db = await PostgresDb.GetInitializedAsync(CancellationToken.None);
        services.AddDbContext<NeoContext>(opts =>
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            opts.UseNpgsql(_db.ConnectionString);
        });
        services.AddScoped<INeoRepository, NeoRepository>();
        services.AddScoped<INeoStatsService, NeoStatsService>();
        
        Services = services;
    }

    public async Task DisposeAsync()
    {
        await _db!.DisposeAsync();
    }
}

[CollectionDefinition(WellKnownConstants.ServiceTemplate)]
public class ServiceProviderCollection : ICollectionFixture<ServiceProviderTemplate>;
