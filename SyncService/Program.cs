using Serilog;
using SyncService.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Debug("Создание билдера");
    var builder = Host.CreateApplicationBuilder(args);
    
    Log.Debug("Инициализация сервисов");
    builder.Services.Initialize();
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services));

    var host = builder.Build();
    Log.Information("Запуск приложения");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Неожиданное завершение работы приложения. Текст ошибки: {ErrorMessage}", ex.Message);
}
finally
{
    Log.CloseAndFlush();
}
