using IncidentRegistrationService.DTOs;
using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;
using IncidentRegistrationService.Services;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace IncidentRegistrationService.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserIncidentRegistrationController : ControllerBase
    {
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IIncidentCorrelationService _incidentCorrelationService;

        private readonly IDatabase _redisGPSStorage;
        private readonly string RedisGeoKey = "device_locations";

        public UserIncidentRegistrationController(IRepository<Incident> incidentRepository,
            IIncidentCorrelationService incidentCorrelationService,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _incidentRepository = incidentRepository;
            _incidentCorrelationService = incidentCorrelationService;
            _redisGPSStorage = connectionMultiplexer.GetDatabase();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewIncident([FromBody] UserIncidentRequest incidentDTO)
        {
            var coordinates = await _redisGPSStorage.GeoPositionAsync(RedisGeoKey, incidentDTO.UserToReport);

            if (coordinates == null)
            {
                return NotFound($"No coordinates registered for the device with token: {incidentDTO.UserToReport}");
            }

            var incident = new Incident
            {
                Category = incidentDTO.Category,
                AreaRadius = 0,
                Timestamp = DateTimeOffset.UtcNow,
                Longitude = coordinates.Value.Longitude,
                Latitude = coordinates.Value.Latitude,
                UserToReport = incidentDTO.UserToReport,
                Description = incidentDTO.Description
            };

            var incidentIsDuplicate = await _incidentCorrelationService.CheckIncidentDuplicate(incident);

            if (incidentIsDuplicate)
            {
                return BadRequest($"The reported incident was already submitted by someone else.");
            }

            incident.Status = "Pending";
            await _incidentRepository.AddAsync(incident);

            return Ok(incident.UserToReport);
        } 
    }
}
