using FluentAssertions;
using NeoTesting.Auxiliary;
using SyncService.DTOs.NeoAnalytics;
using SyncService.Services.NeoAnalytics;

namespace NeoTesting.ServicesTests;

public class NeoAnalyticsServiceTests(ServiceProviderTemplate servicesTemplate)
    : BaseServicesTestClass(servicesTemplate)
{
    #region RequestAndExpected data for NeoAnalyticsService_GetDateRangeAnalyticsAsync_ShouldReturnExpected
    
    public static IEnumerable<object[]> GetDateRangeAnalyticsAsyncRequestAndExpected
    {
        get
        {
            yield return
            [
                "Дефолтные данные",
                new DateTime(2025, 09, 02),
                new DateTime(2025, 09, 07),
                new List<NeoAnalyticsResponse>
                {
                    new ()
                    {
                        Date = new DateTime(2025, 09, 02),
                        Count = 13,
                        MaxDiameter = 0.317716653,
                        MinDiameter = 0.0050647146,
                        AvgVelocity = 49215.46556295043,
                        HazardousCount = 0
                    },
                    new ()
                    {
                        Date = new DateTime(2025, 09, 03),
                        Count = 16,
                        MaxDiameter = 0.5649889822,
                        MinDiameter = 0.0083937296,
                        AvgVelocity = 37397.4795520838,
                        HazardousCount = 0
                    },
                    new ()
                    {
                        Date = new DateTime(2025, 09, 05),
                        Count = 9,
                        MaxDiameter = 0.5445527791,
                        MinDiameter = 0.011131533,
                        AvgVelocity = 53546.341175892754,
                        HazardousCount = 2
                    }
                }
            ];
            
            yield return
            [
                "Запрос на несуществующие в базе даты",
                new DateTime(2024, 09, 02),
                new DateTime(2024, 09, 07),
                new List<NeoAnalyticsResponse>()
            ];
        }
    }
    
    #endregion
    
    [Theory]
    [MemberData(nameof(GetDateRangeAnalyticsAsyncRequestAndExpected))]
    public async Task NeoAnalyticsService_GetDateRangeAnalyticsAsync_ShouldReturnExpected(
        string? testDescription,
        DateTime from,
        DateTime to,
        IEnumerable<NeoAnalyticsResponse> expected,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoAnalyticsService, NeoAnalyticsService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        
        var service = provider.GetRequiredService<INeoAnalyticsService>();
    
        var results = await service
            .GetDateRangeAnalyticsAsync(from, to, cancellationToken);
        
        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }
    
    #region GetHazardousAnalysisAsyncExpected

    public static IEnumerable<object[]> GetHazardousAnalysisAsyncExpected
    {
        get
        {
            yield return
            [
                new List<NeoHazardousAnalysisResponse>
                {
                    new ()
                    {
                        Year = 2025,
                        Month = 10,
                        IsHazardous = false,
                        Count = 66,
                        AvgDiameter = 0.07034525977121213,
                        MaxDiameter = 0.6166517648,
                        AvgVelocity = 45156.51724951473,
                        MinMissDistance = 85900.398455469
                    },
                    new ()
                    {
                        Year = 2025,
                        Month = 9,
                        IsHazardous = true,
                        Count = 8,
                        AvgDiameter = 0.37382558663125004,
                        MaxDiameter = 1.599705282,
                        AvgVelocity = 64767.49092839164,
                        MinMissDistance = 10550794.39910899
                    },
                    new ()
                    {
                        Year = 2025,
                        Month = 10,
                        IsHazardous = true,
                        Count = 3,
                        AvgDiameter = 0.2675523797,
                        MaxDiameter = 0.5675968529,
                        AvgVelocity = 75858.16910810363,
                        MinMissDistance = 12154759.453900844
                    },
                    new ()
                    {
                        Year = 2025,
                        Month = 9,
                        IsHazardous = false,
                        Count = 88,
                        AvgDiameter = 0.06480155252784091,
                        MaxDiameter = 0.5649889822,
                        AvgVelocity = 42498.144495751796,
                        MinMissDistance = 218360.590784648
                    },
                }
            ];
        }
    }
    
    #endregion
    
    [Theory]
    [MemberData(nameof(GetHazardousAnalysisAsyncExpected))]
    public async Task NeoAnalyticsService_GetHazardousAnalysisAsync_ShouldReturnExpected(
        IEnumerable<NeoHazardousAnalysisResponse> expected,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoAnalyticsService, NeoAnalyticsService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        
        var service = provider.GetRequiredService<INeoAnalyticsService>();
    
        var results = await service
            .GetHazardousAnalysisAsync(cancellationToken);
        
        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }
    
    #region  ComparePeriodsAsync test data

    public static IEnumerable<object[]> GetComparePeriodsAsyncTestData
    {
        get
        {
            yield return
            [
                new DateTime(2025, 09, 02),
                new DateTime(2025, 09, 15),
                new DateTime(2025, 09, 16),
                new DateTime(2025, 10, 03),
                new NeoComparisonResponse
                {
                    Count = "48.00%",
                    HazardousCount = "100.00%",
                    AvgDiameter = "-15.23%",
                    MaxDiameter = "0.46%",
                    MinDiameter = "-37.77%",
                    AvgVelocity = "-0.11%",
                    MinMissDistance = "396.89%",
                }
            ];
        }
    }
        
    #endregion
    
    [Theory]
    [MemberData(nameof(GetComparePeriodsAsyncTestData))]
    public async Task NeoAnalyticsService_ComparePeriodsAsync_ShouldReturnExpected(
        DateTime period1Start,
        DateTime period1End,
        DateTime period2Start,
        DateTime period2End,
        NeoComparisonResponse expected,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoAnalyticsService, NeoAnalyticsService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        
        var service = provider.GetRequiredService<INeoAnalyticsService>();

        var results = await service.ComparePeriodsAsync(period1Start, period1End, period2Start, period2End, cancellationToken);
        
        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }
}