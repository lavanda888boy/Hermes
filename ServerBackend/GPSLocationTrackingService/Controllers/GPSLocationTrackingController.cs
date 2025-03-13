using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace GPSLocationTrackingService.Controllers
{
    [ApiController]
    [Route("/")]
    public class GPSLocationTrackingController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redisGPSStorage;

        public GPSLocationTrackingController(IConnectionMultiplexer redisGPSStorage)
        {
            _redisGPSStorage = redisGPSStorage;
        }

        [HttpPost("{deviceToken}")]
        public async Task<IActionResult> SetDeviceGPSLocation(string deviceToken)
        {
            return Ok("");
        }
    }
}
