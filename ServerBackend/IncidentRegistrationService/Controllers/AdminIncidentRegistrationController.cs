using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;
using IncidentRegistrationService.Responses;
using IncidentRegistrationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IncidentRegistrationService.Controllers
{
    [ApiController]
    [Route("/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminIncidentRegistrationController : ControllerBase
    {
        private readonly IRepository<Incident> _incidentRepository;
        private readonly INotificationTransmissionService _notificationTransmissionService;

        public AdminIncidentRegistrationController(IRepository<Incident> incidentRepository,
            INotificationTransmissionService notificationTransmissionService) 
        { 
            _incidentRepository = incidentRepository;
            _notificationTransmissionService = notificationTransmissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncidentsForValidation()
        {
            var incidents = await _incidentRepository.GetFilteredAsync("Pending");

            var incidentDtos = incidents.Select(incident => new AdminIncidentResponse
            {
                Id = incident.Id,
                Category = incident.Category,
                Timestamp = incident.Timestamp,
                Longitude = incident.Longitude,
                Latitude = incident.Latitude,
                UserToReport = incident.UserToReport,
                Description = incident.Description,
            }).ToList();

            return Ok(incidentDtos);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewIncident([FromBody] Incident incident)
        {
            await _incidentRepository.AddAsync(incident);

            var notificationData = new Dictionary<string, string>()
            {
                { "Category", incident.Category },
                { "Severity", Enum.GetName(typeof(IncidentSeverity), incident.Severity) },
                { "Description", incident.Description }
            };

            await _notificationTransmissionService.SendIncidentNotification(notificationData);

            return Ok(notificationData);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrValidateIncidentDetails([FromBody] Incident incident)
        {
            var incidentToUpdate = await _incidentRepository.GetByIdAsync(incident.Id);

            if (incidentToUpdate == null)
            {
                return BadRequest($"Cannot update or validate non-existing incident with id: {incident.Id}");
            }

            await _incidentRepository.UpdateAsync(incidentToUpdate);

            var notificationData = new Dictionary<string, string>()
            {
                { "Category", incidentToUpdate.Category },
                { "Severity", Enum.GetName(typeof(IncidentSeverity), incidentToUpdate.Severity) },
                { "Description", incidentToUpdate.Description }
            };

            await _notificationTransmissionService.SendIncidentNotification(notificationData);

            return Ok(incidentToUpdate.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredIncident(string id)
        {
            var incidentToDelete = await _incidentRepository.GetByIdAsync(id);

            if (incidentToDelete == null)
            {
                return BadRequest($"There is no registered incident with id: {id}");
            }

            await _incidentRepository.DeleteAsync(incidentToDelete);
            
            return Ok(id);
        }
    }
}
