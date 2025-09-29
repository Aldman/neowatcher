using Newtonsoft.Json;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;
using SyncService.DTOs.NeoSearch;

namespace SyncService.Helpers;

public static class CacheKeyGenerator
{
    public static string Generate() => "constQuery";
    public static string Generate(DateTime from, DateTime to) => $"{from} : {to}";
    public static string Generate(DateTime from, DateTime to, DateTime from2, DateTime to2) 
        => $"{from} : {to}; {from2} : {to2}";
    public static string Generate(NeoFilterRequest request) => JsonConvert.SerializeObject(request);
    public static string Generate(NeoSearchRequest request) => JsonConvert.SerializeObject(request);
}