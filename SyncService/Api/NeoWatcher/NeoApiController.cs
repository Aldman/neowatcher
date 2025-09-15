using Microsoft.AspNetCore.Mvc;
using SyncService.Api.NeoWatcher.NeoFilterRequestParts;

namespace SyncService.Api.NeoWatcher;

[ApiController]
[Route("neo")]
public class NeoApiController : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromQuery] NeoFilterRequest filter)
    {
        

        return Ok();
    }
}