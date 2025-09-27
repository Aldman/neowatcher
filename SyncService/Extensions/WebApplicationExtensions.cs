using SyncService.Middlewares;

namespace SyncService.Extensions;

public static class WebApplicationExtensions
{
    public static void Initialize(this WebApplication app)
    {
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseExceptionHandling();
    }
}