using Serilog;
using SyncService.Extensions;
using SyncService.Middlewares;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Debug("Создание билдера");
    var builder = WebApplication.CreateBuilder(args);
    
    Log.Debug("Инициализация сервисов");
    builder.Services.Initialize();
    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services));

    var app = builder.Build();
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandling();
    
    Log.Information("Запуск приложения");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Неожиданное завершение работы приложения. Текст ошибки: {ErrorMessage}", ex.Message);
}
finally
{
    Log.Information(Environment.NewLine);
    Log.CloseAndFlush();
}
