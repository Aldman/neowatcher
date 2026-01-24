using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SyncService.EfComponents;

namespace NeoTesting.Auxiliary;

public class CustomWebApplicationFactory : WebApplicationFactory<SyncService.Program>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = null!;
    
    private PostgresDb _db;
    
    public CustomWebApplicationFactory()
    {
        _db = PostgresDb.GetInitializedAsync(CancellationToken.None)
            .GetAwaiter()
            .GetResult();
    }
    
    public async Task InitializeAsync()
    {
        HttpClient = CreateClient();

        await LoadDbDump();
    }

    private async Task LoadDbDump()
    {
        const string targetMigrationName = "20250916094643_RemoveNotMappedAttribute";
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NeoContext>();

        var applied = await dbContext.Database.GetAppliedMigrationsAsync();
        if (applied.Contains(targetMigrationName))
            return;
        
        await dbContext.Database.MigrateAsync(targetMigration: targetMigrationName);

        await using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();
        var sql = await File.ReadAllTextAsync(Path.Combine("Resources", "testDataBackup.sql"));
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    public new async Task DisposeAsync()
    {
        await _db.DisposeAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove(services.FirstOrDefault(service => typeof(NeoContext) == service.ServiceType));
            services.Remove(services.FirstOrDefault(service => typeof(DbContextOptions<NeoContext>) == service.ServiceType));
            services.AddDbContext<NeoContext>((_, option) =>
            {
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
                option.UseNpgsql(_db.ConnectionString);
            });
        });
    }
}

[CollectionDefinition(WellKnownConstants.ApplicationFactory)]
public class ApplicationFactoryCollection : ICollectionFixture<CustomWebApplicationFactory>;