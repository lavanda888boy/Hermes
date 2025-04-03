using IncidentRegistrationService.DTOs;
using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;
using IncidentRegistrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IncidentRegistrationService.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserIncidentRegistrationController : ControllerBase
    {
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IIncidentCorrelationService _incidentCorrelationService;

        public UserIncidentRegistrationController(IRepository<Incident> incidentRepository,
            IIncidentCorrelationService incidentCorrelationService)
        {
            _incidentRepository = incidentRepository;
            _incidentCorrelationService = incidentCorrelationService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewIncident([FromBody] UserIncidentRequest incidentDTO)
        {
            var incident = new Incident
            {
                Category = incidentDTO.Category,
                AreaRadius = 0,
                Timestamp = DateTimeOffset.UtcNow,
                Longitude = incidentDTO.Longitude,
                Latitude = incidentDTO.Latitude,
                UserToReport = incidentDTO.UserToReport
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
