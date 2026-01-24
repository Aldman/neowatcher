using Microsoft.EntityFrameworkCore;
using SyncService.EfComponents.DbSets;

namespace SyncService.EfComponents;

public class NeoContext(DbContextOptions<NeoContext> options) : DbContext(options)
{
    public DbSet<DbNearEarthObject> NearEarthObjects { get; set; } = null!;
    public DbSet<DbCloseApproachData> CloseApproachData { get; set; } = null!;
    public DbSet<SyncDateTimes> SyncDateTimes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbNearEarthObject>()
            .HasOne(x => x.CloseApproachData)
            .WithOne(x => x.NearEarthObject)
            .HasForeignKey<DbNearEarthObject>(x => x.CloseApproachDataId);
    }
}