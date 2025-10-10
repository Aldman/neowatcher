using FluentAssertions;
using NeoTesting.Auxiliary;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.NeoWatcherApi.Controllers.NeoStats;
using SyncService.Services.NeoStats;

namespace NeoTesting;

[Collection(WellKnownConstants.ServiceTemplate)]
public class NeoStatsServiceTests
{
    private readonly ServiceProviderTemplate _servicesTemplate;

    public NeoStatsServiceTests(ServiceProviderTemplate servicesTemplate)
    {
        _servicesTemplate = servicesTemplate;
    }
    
    #region NeoStatService test data

    public static IEnumerable<object[]> RequestAndExpected
    {
        get
        {
            yield return
            [
                "Дефолтный запрос с минимумом параметров и минимальным ответом",
                new NeoFilterRequest
                {
                    From = new DateTime(2025, 9, 2),
                    To = new DateTime(2025, 9, 15),
                    IsHazardous = true
                },
                
                new List<NeoStatsResponse>
                {
                    new()
                    {
                        Date = new DateTime(2025, 9, 5),
                        ObjectCount = 2,
                        MaxDiameter = 0.5445527791,
                        AvgVelocity = 37585.71006047425,
                        HasHazardousObjects = true
                    },
                    new()
                    {
                        Date = new DateTime(2025, 9, 8),
                        ObjectCount = 1,
                        MaxDiameter = 0.2910985075,
                        AvgVelocity = 106728.6488688101,
                        HasHazardousObjects = true
                    }
                }
            ];
            
            yield return
            [
                "Тот же запрос, но с прямой сортировкой по количеству",
                new NeoFilterRequest
                {
                    From = new DateTime(2025, 9, 2),
                    To = new DateTime(2025, 9, 15),
                    IsHazardous = true,
                    SortBy = SortBy.Count,
                    SortDirection = SortDirections.Asc
                },
                
                new List<NeoStatsResponse>
                {
                    new()
                    {
                        Date = new DateTime(2025, 9, 8),
                        ObjectCount = 1,
                        MaxDiameter = 0.2910985075,
                        AvgVelocity = 106728.6488688101,
                        HasHazardousObjects = true
                    },
                    new()
                    {
                        Date = new DateTime(2025, 9, 5),
                        ObjectCount = 2,
                        MaxDiameter = 0.5445527791,
                        AvgVelocity = 37585.71006047425,
                        HasHazardousObjects = true
                    }
                }
            ];
            
            yield return
            [
                "Обратная сортировка по количеству",
                new NeoFilterRequest
                {
                    From = new DateTime(2025, 9, 2),
                    To = new DateTime(2025, 9, 15),
                    IsHazardous = true,
                    SortBy = SortBy.Count,
                    SortDirection = SortDirections.Asc
                },
                
                new List<NeoStatsResponse>
                {
                    new()
                    {
                        Date = new DateTime(2025, 9, 8),
                        ObjectCount = 1,
                        MaxDiameter = 0.2910985075,
                        AvgVelocity = 106728.6488688101,
                        HasHazardousObjects = true
                    },
                    new()
                    {
                        Date = new DateTime(2025, 9, 5),
                        ObjectCount = 2,
                        MaxDiameter = 0.5445527791,
                        AvgVelocity = 37585.71006047425,
                        HasHazardousObjects = true
                    }
                }
            ];
        }
    }
    
    #endregion
    
    // В xUnit нет возможности задавать имя теста для списка тест-параметров как в NUnit
    // (например, yield return new TestCaseData(12, 3).Returns(4).SetName("Simple params").
    // Поэтому я буду передавать описание параметров 1ым аргументом, чтобы он отображался в test explorer 
    [Theory]
    [MemberData(nameof(RequestAndExpected))]
    public async Task NeoStatService_GetFilteredStatsAsync_ShouldReturnExpected(
        string? testDescription,
        NeoFilterRequest request,
        IEnumerable<NeoStatsResponse> expectedStats,
        CancellationToken cancellationToken = default)
    {
        _servicesTemplate.Services!.AddTransient<INeoStatsService, NeoStatsService>();
        var provider = await _servicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        
        var statsService = provider.GetRequiredService<INeoStatsService>();

        var results = await statsService
            .GetFilteredStatsAsync(request, cancellationToken);
        
        results.Should().BeEquivalentTo(expectedStats, opts => opts.WithStrictOrdering());
    }
}