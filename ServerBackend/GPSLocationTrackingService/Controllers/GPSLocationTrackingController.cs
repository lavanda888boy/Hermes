using GPSLocationTrackingService.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace GPSLocationTrackingService.Controllers
{
    [ApiController]
    [Route("/")]
    public class GPSLocationTrackingController : ControllerBase
    {
        private readonly IDatabase _redisGPSStorage;
        private readonly string RedisGeoKey = "device_locations";

        public GPSLocationTrackingController(IConnectionMultiplexer redisResource)
        {
            _redisGPSStorage = redisResource.GetDatabase();
        }

        [HttpGet("{deviceToken}")]
        public async Task<IActionResult> GetDeviceGPSLocationByDeviceToken(string deviceToken)
        {
            var coordinates = await _redisGPSStorage.GeoPositionAsync(RedisGeoKey, deviceToken);
            
            if (coordinates.HasValue)
            {
                return Ok(coordinates.Value);
            }

            return BadRequest($"No coordinates registered for the device with token: {deviceToken}");
        }

        [HttpPost("{deviceToken}")]
        public async Task<IActionResult> SetDeviceGPSLocation(string deviceToken, [FromBody] GPSLocation gpsLocation)
        {
            await _redisGPSStorage.GeoAddAsync(RedisGeoKey, gpsLocation.Longitude, gpsLocation.Latitude, deviceToken);
            return Ok(deviceToken);
        }
    }
}
