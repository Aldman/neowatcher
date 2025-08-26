using SyncService.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Initialize();

var host = builder.Build();
host.Run();
