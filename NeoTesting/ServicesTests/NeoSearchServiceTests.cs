using FluentAssertions;
using NeoTesting.Auxiliary;
using SyncService.DTOs.NeoSearch;
using SyncService.Services.NeoSearch;

namespace NeoTesting.ServicesTests;

public class NeoSearchServiceTests(ServiceProviderTemplate servicesTemplate) : BaseServicesTestClass(servicesTemplate)
{
    #region SearchAsync test data

    public static IEnumerable<object?[]> SearchRequestAndExpectedReport
    {
        get
        {
            yield return
            [
                "Поиск по частичному совпадению имени",
                new NeoSearchRequest
                {
                    Name = "gq"
                },
                
                new List<NeoSearchResult>
                {
                    new ()
                    {
                        Id = "54526130",
                        Name = "(2025 GQ)",
                        Diameter = 0.22990192985000002,
                        IsHazardous = false,
                        Velocity = 54241.9279072477,
                        MissDistance = 26714054.001860715,
                        SimilarityScore = 100
                    }
                }
            ];

            yield return
            [
                "Поиск с пустым результатом",
                new NeoSearchRequest
                {
                    Name = "Non existent result"
                },
                
                null
            ];
        }
    }

    #endregion

    [Theory]
    [MemberData(nameof(SearchRequestAndExpectedReport))]
    public async Task NeoSearchService_SearchAsync_ShouldReturnExpected(
        string? description,
        NeoSearchRequest request,
        IEnumerable<NeoSearchResult>? expected,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoSearchService, NeoSearchService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);

        var service = provider.GetRequiredService<INeoSearchService>();

        var results = await service.SearchAsync(request, cancellationToken);

        results.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }
    
    #region SearchAsync test data for count comparison

    public static IEnumerable<object?[]> SearchRequestAndExpectedCount
    {
        get
        {
            yield return
            [
                "Задание лимита",
                new NeoSearchRequest
                {
                    Limit = 7
                },
                
                7
            ];
            
            yield return
            [
                "Задание размера страницы",
                new NeoSearchRequest
                {
                    PageSize = 20
                },
                
                20
            ];
        }
    }

    #endregion
    
    [Theory]
    [MemberData(nameof(SearchRequestAndExpectedCount))]
    public async Task NeoSearchService_SearchAsync_ShouldReturnExpectedCount(
        string? description,
        NeoSearchRequest request,
        int expectedCount,
        CancellationToken cancellationToken = default)
    {
        ServicesTemplate.Services!.AddTransient<INeoSearchService, NeoSearchService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);

        var service = provider.GetRequiredService<INeoSearchService>();

        var results = await service.SearchAsync(request, cancellationToken);

        results.Should().HaveCount(expectedCount);
    }
    
    [Theory]
    [InlineData("54543395", 50)]
    [InlineData("3840783", 50)]
    [InlineData("50", 0)]
    public async Task NeoSearchService_FindSimilar_ShouldReturnExpectedCount(
        string neoId,
        int expectedCount,
        CancellationToken cancellationToken = default)
    {
        //Arrange
        ServicesTemplate.Services!.AddTransient<INeoSearchService, NeoSearchService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        var service = provider.GetRequiredService<INeoSearchService>();

        // Act
        var results = await service.FindSimilarAsync(neoId, cancellationToken);
        var actualCount = results?.Count() ?? 0;

        // Assert
        Assert.Equal(expectedCount, actualCount);
    }
    
    [Theory]
    [InlineData("gq", 7)]
    [InlineData("rb", 7)]
    [InlineData("non existent name", 0)]
    public async Task NeoSearchService_GetSuggestionsAsync_ShouldNotBeMoreThanLimitAndContainsSearchPart(
        string searchPart,
        int limit,
        CancellationToken cancellationToken = default)
    {
        //Arrange
        ServicesTemplate.Services!.AddTransient<INeoSearchService, NeoSearchService>();
        var provider = await ServicesTemplate.GetServiceProviderWithInitializedDbAsync(cancellationToken);
        var service = provider.GetRequiredService<INeoSearchService>();

        // Act
        var results = await service.GetSuggestionsAsync(searchPart, limit, cancellationToken);
        var actualCount = results?.Count() ?? 0;

        // Assert
        actualCount.Should().BeLessThanOrEqualTo(limit);
        results?.Should().Match(x => x.All(s => s.Contains(searchPart, StringComparison.InvariantCultureIgnoreCase)));
    }
}