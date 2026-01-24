using FluentAssertions;
using NeoTesting.Auxiliary;
using SyncService.DTOs.NeoStatistics;
using SyncService.Services.NeoStatistics;

namespace NeoTesting.ServicesTests;

public class NeoStatisticsServiceTests(ServiceProviderTemplate servicesTemplate)
    : BaseServicesTestClass(servicesTemplate)
{
    #region GetBasicStatisticsAsync test data

    public static IEnumerable<object?[]> DateRangeAndExpectedResponse
    {
        get
        {
            yield return
            [
                "Стандартный запрос с ответом",
                new DateTime(2025, 09, 07),
                new DateTime(2025, 09, 30),
                
                new NeoBasicStatsResponse
                {
                    TotalCount = 58,
                    AverageDiameter = 0.09096664478620692,
                    MedianDiameter = 0.0493790371,
                    StandardDeviationDiameter = 0.15973552433441113,
                    AverageVelocity = 43756.8766779062,
                    MedianVelocity = 35684.7159731211,
                    StandardDeviationVelocity = 27331.605278472925,
                    HazardousCount = 6,
                    HazardousPercentage = 0.10344827586206896
                }
            ];

            yield return
            [
                "Запрос с пустым ответом",
                new DateTime(2009, 09, 07),
                new DateTime(2010, 09, 30),
                
                null
            ];
        }
    }

    #endregion

    [Theory]
    [MemberData(nameof(DateRangeAndExpectedResponse))]
    public async Task NeoStatisticsService_GetBasicStatisticsAsync_ShouldReturnExpected(
        string? description,
        DateTime startDate,
        DateTime endDate,
        NeoBasicStatsResponse expected,
        CancellationToken cancellationToken = default
        )
    {
        // Arrange
        ServicesTemplate.Services!.AddTransient<INeoStatisticsService, NeoStatisticsService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        var service = provider.GetRequiredService<INeoStatisticsService>();

        // Act
        var results = await service.GetBasicStatisticsAsync(startDate, endDate, cancellationToken);

        // Assert
        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }
    
    #region GetDiameterDistributionAsync test data

    public static IEnumerable<object?[]> BucketsCountAndExpectedResponse
    {
        get
        {
            yield return
            [
                10,
                
                new List<NeoDistributionResponse>
                {
                    new()
                    {
                        MinValue = 0.003745783,
                        MaxValue = 0.119126966345,
                        Count = 132,
                        Percentage = 0.8
                    },
                    new()
                    {
                        MinValue = 0.119126966345,
                        MaxValue = 0.23450814969,
                        Count = 21,
                        Percentage = 0.12727272727272726
                    },
                    new()
                    {
                        MinValue = 0.23450814969,
                        MaxValue = 0.34988933303499997,
                        Count = 4,
                        Percentage = 0.024242424242424242
                    },
                    new()
                    {
                        MinValue = 0.34988933303499997,
                        MaxValue = 0.46527051638,
                        Count = 7,
                        Percentage = 0.04242424242424243
                    },
                    new()
                    {
                        MinValue = 0.46527051638,
                        MaxValue = 0.580651699725,
                        Count = 0,
                        Percentage = 0
                    },
                    new()
                    {
                        MinValue = 0.580651699725,
                        MaxValue = 0.6960328830699999,
                        Count = 0,
                        Percentage = 0
                    },
                    new()
                    {
                        MinValue = 0.6960328830699999,
                        MaxValue = 0.8114140664149999,
                        Count = 0,
                        Percentage = 0
                    },
                    new()
                    {
                        MinValue = 0.8114140664149999,
                        MaxValue = 0.9267952497599999,
                        Count = 0,
                        Percentage = 0
                    },
                    new()
                    {
                        MinValue = 0.9267952497599999,
                        MaxValue = 1.042176433105,
                        Count = 0,
                        Percentage = 0
                    },
                    new()
                    {
                        MinValue = 1.042176433105,
                        MaxValue = 1.15755761645,
                        Count = 1,
                        Percentage = 0.006060606060606061
                    },
                }
            ];
        }
    }

    #endregion

    [Theory]
    [MemberData(nameof(BucketsCountAndExpectedResponse))]
    public async Task NeoStatisticsService_GetDiameterDistributionAsync_ShouldReturnExpected(
        int buckets,
        IEnumerable<NeoDistributionResponse> expected,
        CancellationToken cancellationToken = default
        )
    {
        // Arrange
        ServicesTemplate.Services!.AddTransient<INeoStatisticsService, NeoStatisticsService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        var service = provider.GetRequiredService<INeoStatisticsService>();

        // Act
        var results = await service.GetDiameterDistributionAsync(buckets, cancellationToken);

        // Assert
        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }

    #region DetectAnomaliesExpected

    public static IEnumerable<object?[]> DetectAnomaliesExpected
    {
        get
        {
            yield return
            [
                new List<NeoAnomalyResponse>
                {
                    new()
                    {
                        Id = "3467394",
                        Name = "(2009 SG18)",
                        Diameter = 1.15755761645,
                        Velocity = 123600.5022445999,
                        MissDistance = 17071556.983058635,
                        AnomalyReasons = ["Diameter high (z=8.63)"],
                        AnomalyScore = 8.6288061287613
                    },
                    new()
                    {
                        Id = "3612164",
                        Name = "(2012 TA259)",
                        Diameter = 0.36437000355000004,
                        Velocity = 131171.8631870657,
                        MissDistance = 70170345.77088325,
                        AnomalyReasons = ["Velocity high (z=3.26)"],
                        AnomalyScore = 3.25999446914541
                    },
                }
            ];
        }
    }

    #endregion

    [Theory]
    [MemberData(nameof(DetectAnomaliesExpected))]
    public async Task NeoStatisticsService_DetectAnomaliesAsync_ShouldReturnExpected(
        IEnumerable<NeoAnomalyResponse> expected,
        CancellationToken cancellationToken = default)
    {
        // Arrange
        ServicesTemplate.Services!.AddTransient<INeoStatisticsService, NeoStatisticsService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        var service = provider.GetRequiredService<INeoStatisticsService>();

        // Act
        var results = await service.DetectAnomaliesAsync(cancellationToken);

        // Assert
        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }
}