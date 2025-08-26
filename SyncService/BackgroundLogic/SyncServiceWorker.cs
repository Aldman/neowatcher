namespace SyncService.BackgroundLogic;

public class SyncServiceWorker : BackgroundService
{
    private readonly ILogger<SyncServiceWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _syncPeriod =  TimeSpan.FromDays(1);

    public SyncServiceWorker(ILogger<SyncServiceWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var sync = scope.ServiceProvider.GetRequiredService<SyncJob>();

            try
            {
                await sync.UpdateDataOfNasaAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            
            await Task.Delay(_syncPeriod, stoppingToken);
        }
    }
}
