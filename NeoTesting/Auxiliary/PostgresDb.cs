using Npgsql;
using Testcontainers.PostgreSql;

namespace NeoTesting.Auxiliary;

public class PostgresDb
{
    private PostgresDb()
    {
    }

    private PostgreSqlContainer Container { get; } = new PostgreSqlBuilder()
        .WithImage("postgres:17.6-alpine")
        .WithDatabase("test")
        .WithUsername("admin")
        .WithPassword("admin")
        .WithPortBinding(5434, 5432)
        .WithCleanUp(true)
        .Build();

    public string ConnectionString => Container.GetConnectionString();

    private async Task InitializeAsync(CancellationToken ct = default)
    {
        await Container.StartAsync(ct);
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync(ct);
    }

    public async Task DisposeAsync() => await Container.DisposeAsync();

    public static async Task<PostgresDb> GetInitializedAsync(CancellationToken ct = default)
    {
        var fixture = new PostgresDb();
        
        await fixture.InitializeAsync(ct);
        
        return fixture;
    }
}