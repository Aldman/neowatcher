using Newtonsoft.Json;
using SyncService.NeoApiComponents.Main;

namespace SyncService.Extensions;

public static class NeoResponseExtensions
{
    public static string ToJson(this NeoResponse self) => JsonConvert.SerializeObject(self);
}