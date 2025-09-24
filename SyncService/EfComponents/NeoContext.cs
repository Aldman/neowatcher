using Microsoft.EntityFrameworkCore;
using SyncService.Constants;
using SyncService.EfComponents.DbSets;

namespace SyncService.EfComponents;

public class NeoContext : DbContext
{
    public DbSet<DbNearEarthObject> NearEarthObjects { get; set; } = null!;
    public DbSet<DbCloseApproachData> CloseApproachData { get; set; } = null!;
    public DbSet<SyncDateTimes> SyncDateTimes { get; set; } = null!;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbNearEarthObject>()
            .HasOne(x => x.CloseApproachData)
            .WithOne(x => x.NearEarthObject)
            .HasForeignKey<DbNearEarthObject>(x => x.CloseApproachDataId);
    }
}