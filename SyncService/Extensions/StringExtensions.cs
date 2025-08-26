using Newtonsoft.Json;
using SyncService.NeoApiComponents.Main;

namespace SyncService.Extensions;

public static class StringExtensions
{
    public static NeoResponse? ToNeoResponse(this string json) => JsonConvert.DeserializeObject<NeoResponse>(json);
}