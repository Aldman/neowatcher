using Newtonsoft.Json;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;

namespace SyncService.Helpers;

public static class CacheKeyGenerator
{
    public static string Generate(DateTime from, DateTime to) => $"{from} : {to}";
    public static string Generate(NeoFilterRequest request) => JsonConvert.SerializeObject(request);
}