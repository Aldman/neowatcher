using FluentAssertions;
using NeoTesting.Auxiliary;
using SyncService.DTOs.NeoReporting;
using SyncService.Services.NeoReporting;

namespace NeoTesting.ServicesTests;

public class NeoReportingServiceTests(ServiceProviderTemplate servicesTemplate)
    : BaseServicesTestClass(servicesTemplate)
{
    #region Daily report test data

    public static IEnumerable<object?[]> RequestDateAndExpectedReport
    {
        get
        {
            yield return
            [
                "Дефолтный запрос с ответом",
                new DateTime(2025, 09, 05),

                new DailyReportResponse
                {
                    Date = new DateTime(2025, 09, 05),
                    TotalCount = 9,
                    HazardousCount = 2,
                    AverageDiameter = 0.142974976,
                    MaxDiameter = 0.5445527791,
                    HourlyStats =
                    [
                        new HourlyStats
                        {
                            Hour = 6,
                            TotalCount = 1,
                            HazardousCount = 0,
                            AverageDiameter = 0.021161313299999998,
                            MaxDiameter = 0.0292442157
                        },
                        new HourlyStats
                        {
                            Hour = 9,
                            TotalCount = 1,
                            HazardousCount = 0,
                            AverageDiameter = 0.05991597305,
                            MaxDiameter = 0.0828018383
                        },
                        new HourlyStats
                        {
                            Hour = 10,
                            TotalCount = 1,
                            HazardousCount = 0,
                            AverageDiameter = 0.1424106986,
                            MaxDiameter = 0.1968067451
                        },
                        new HourlyStats
                        {
                            Hour = 11,
                            TotalCount = 1,
                            HazardousCount = 0,
                            AverageDiameter = 0.0180111987,
                            MaxDiameter = 0.0248908644
                        },
                        new HourlyStats
                        {
                            Hour = 14,
                            TotalCount = 1,
                            HazardousCount = 0,
                            AverageDiameter = 0.1657840011,
                            MaxDiameter = 0.2291078547
                        },
                        new HourlyStats
                        {
                            Hour = 16,
                            TotalCount = 1,
                            HazardousCount = 0,
                            AverageDiameter = 0.04202837675,
                            MaxDiameter = 0.0580817882
                        },
                        new HourlyStats
                        {
                            Hour = 17,
                            TotalCount = 2,
                            HazardousCount = 2,
                            AverageDiameter = 0.3940420927,
                            MaxDiameter = 0.5445527791
                        },
                        new HourlyStats
                        {
                            Hour = 22,
                            TotalCount = 1,
                            HazardousCount = 0,
                            AverageDiameter = 0.0493790371,
                            MaxDiameter = 0.0682401509
                        },
                    ]
                }
            ];

            yield return
            [
                "Запрос с пустым ответом",
                new DateTime(2025, 09, 07),

                null
            ];
        }
    }

    #endregion

    [Theory]
    [MemberData(nameof(RequestDateAndExpectedReport))]
    public async Task NeoReportingService_GetDailyReportAsync_ShouldReturnExpected(
        string? description,
        DateTime date,
        DailyReportResponse? expected,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoReportingService, NeoReportingService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);

        var service = provider.GetRequiredService<INeoReportingService>();

        var results = await service
            .GetDailyReportAsync(date, cancellationToken);

        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }

    #region GetWeeklyReportAsync test data

    public static IEnumerable<object?[]> WeekStartAndExpectedReport
    {
        get
        {
            yield return
            [
                "Стандартный запрос с ответом",
                new DateTime(2025, 09, 15),

                new WeeklyReportResponse
                {
                    DateStart = new DateTime(2025, 09, 15),
                    DateEnd = new DateTime(2025, 09, 22),
                    TotalCount = 24,
                    HazardousCount = 1,
                    DailyStats =
                    [
                        new DailyStats
                        {
                            Date = new DateTime(2025, 09, 15),
                            TotalCount = 9,
                            HazardousCount = 1,
                            AverageDiameter = 0.18258995766666666,
                            MaxDiameter = 1.599705282
                        },
                        new DailyStats
                        {
                            Date = new DateTime(2025, 09, 16),
                            TotalCount = 15,
                            HazardousCount = 0,
                            AverageDiameter = 0.05871611350333332,
                            MaxDiameter = 0.2060819612
                        }
                    ],
                    Comparison = new WeeklyComparison
                    {
                        CurrentTotal = 24,
                        PreviousTotal = 21,
                        CurrentHazardous = 1,
                        PreviousHazardous = 2,
                        DeltaTotal = 3,
                        DeltaHazardous = -1,
                        PercentChangeTotal = 0.14285714285714285,
                        PercentChangeHazardous = -0.5,
                    }
                }
            ];

            yield return
            [
                "Запрос с пустым ответом",
                new DateTime(2015, 09, 15),

                null
            ];
        }
    }

    #endregion

    [Theory]
    [MemberData(nameof(WeekStartAndExpectedReport))]
    public async Task NeoReportingService_GetWeeklyReportAsync_ShouldReturnExpected(
        string? description,
        DateTime weekStart,
        WeeklyReportResponse? expected,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoReportingService, NeoReportingService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);

        var service = provider.GetRequiredService<INeoReportingService>();

        var results = await service
            .GetWeeklyReportAsync(weekStart, cancellationToken);

        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }

    #region GetSummaryReportAsync test data

    public static IEnumerable<object?[]> SummaryRequestAndExpectedReport
    {
        get
        {
            yield return
            [
                "Стандартный запрос с ответом",
                new DateTime(2025, 09, 07),
                new DateTime(2025, 09, 10),

                new SummaryReportResponse
                {
                    From = new DateTime(2025, 09, 07),
                    To = new DateTime(2025, 09, 10),
                    TotalCount = 12,
                    HazardousCount = 1,
                    TopHazardous =
                    [
                        new TopHazardousObject
                        {
                            Id = "3046125",
                            Name = "(2000 QV7)",
                            Diameter = 0.21064085885,
                            Velocity = 106728.6488688101,
                            MissDistance = 68214230.50158624,
                            CloseApproachDate = new DateTime(2025, 09, 08, 6, 25, 0),
                            DangerScore = 0.00032957073760589385
                        }
                    ],
                    Trends =
                    [
                        new TrendData
                        {
                            PeriodStart = new DateTime(2025, 09, 08),
                            TotalCount = 12,
                            HazardousCount = 1,
                            AvgDiameter = 0.058751395474999996
                        }
                    ]
                }
            ];

            yield return
            [
                "Запрос с пустым ответом",
                new DateTime(2007, 09, 07),
                new DateTime(2010, 09, 10),

                null
            ];
        }
    }

    #endregion

    [Theory]
    [MemberData(nameof(SummaryRequestAndExpectedReport))]
    public async Task NeoReportingService_GetSummaryReportAsync_ShouldReturnExpected(
        string? description,
        DateTime dateStart,
        DateTime dateEnd,
        SummaryReportResponse? expected,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoReportingService, NeoReportingService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);

        var service = provider.GetRequiredService<INeoReportingService>();

        var results = await service
            .GetSummaryReportAsync(dateStart, dateEnd, cancellationToken);

        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }
}