using Microsoft.EntityFrameworkCore;
using SyncService.Constants;
using SyncService.NeoApiComponents;
using SyncService.NeoApiComponents.Main;

namespace SyncService.EfComponents;

public class NeoContext : DbContext
{
    public DbSet<SimplifiedNearEarthObject> NearEarthObjects { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString(WellKnownNames.DefaultConnection);
        
        optionsBuilder.UseNpgsql(connectionString);
    }
}