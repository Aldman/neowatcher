using Serilog;

namespace SyncService.BackgroundLogic;

public class SyncServiceWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _syncPeriod =  TimeSpan.FromDays(1);

    public SyncServiceWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Запуск {ClassName}", nameof(SyncServiceWorker));
        
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
                Log.Error(exception: ex,
                    messageTemplate: "Ошибка при запуске {ErrorClass}: {ErrorMessage}",
                    propertyValue0: nameof(SyncJob),
                    propertyValue1: ex.Message);
            }
            
            await Task.Delay(_syncPeriod, stoppingToken);
        }
    }
}
