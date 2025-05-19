using IncidentRegistrationService.DTOs;
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
        public async Task<IActionResult> GetAllIncidentsByStatus([FromQuery] string status)
        {
            var incidents = await _incidentRepository.GetFilteredAsync(status);

            var incidentDtos = incidents.Select(incident => new AdminIncidentResponse
            {
                Id = incident.Id,
                Category = incident.Category,
                Severity = Enum.GetName(typeof(IncidentSeverity), incident.Severity),
                AreaRadius = incident.AreaRadius,
                Timestamp = incident.Timestamp,
                Longitude = incident.Longitude,
                Latitude = incident.Latitude,
                UserToReport = incident.UserToReport,
                Description = incident.Description,
            }).ToList();

            return Ok(incidentDtos);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewIncident([FromBody] AdminIncidentRequest incidentDTO)
        {
            var incident = new Incident
            {
                Category = incidentDTO.Category,
                Severity = Enum.Parse<IncidentSeverity>(incidentDTO.Severity),
                AreaRadius = incidentDTO.AreaRadius,
                Timestamp = DateTimeOffset.UtcNow,
                Longitude = incidentDTO.Longitude,
                Latitude = incidentDTO.Latitude,
                UserToReport = "Admin",
                Status = "Approved",
                Description = incidentDTO.Description
            };

            var newIncident = await _incidentRepository.AddAsync(incident);
            await _notificationTransmissionService.SendIncidentNotification(incident);

            return Ok(newIncident.Id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrValidateIncidentDetails([FromBody] AdminIncidentRequest incidentDTO,
            [FromQuery] string status = "Validate")
        {
            var incidentToUpdate = await _incidentRepository.GetByIdAsync(incidentDTO.Id);

            if (incidentToUpdate == null)
            {
                return BadRequest($"Cannot update or validate non-existing incident with id: {incidentDTO.Id}");
            }

            incidentToUpdate.Category = incidentDTO.Category;
            incidentToUpdate.Severity = Enum.Parse<IncidentSeverity>(incidentDTO.Severity);
            incidentToUpdate.AreaRadius = incidentDTO.AreaRadius;
            incidentToUpdate.Longitude = incidentDTO.Longitude;
            incidentToUpdate.Latitude = incidentDTO.Latitude;
            incidentToUpdate.Status = "Approved";
            incidentToUpdate.Description = incidentDTO.Description;

            if (status != "Validate")
            {
                incidentToUpdate.Timestamp = DateTimeOffset.UtcNow;
            }

            await _incidentRepository.UpdateAsync(incidentToUpdate);

            if (status != "Validate")
            {
                await _notificationTransmissionService.SendIncidentNotification(incidentToUpdate, "This is a follow up notification.");
            } 
            else
            {
                await _notificationTransmissionService.SendIncidentNotification(incidentToUpdate);
            }

            return Ok(incidentToUpdate.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegisteredIncident(string id, [FromQuery] string actor = "User")
        {
            var incidentToDelete = await _incidentRepository.GetByIdAsync(id);

            if (incidentToDelete == null)
            {
                return BadRequest($"There is no registered incident with id: {id}");
            }

            if (actor != "User")
            {
                incidentToDelete.Timestamp = DateTimeOffset.UtcNow;
            }

            await _incidentRepository.DeleteAsync(incidentToDelete);

            if (actor != "User")
            {
                string note = "This incident was added by mistake or no longer affects the area.";
                await _notificationTransmissionService.SendIncidentNotification(incidentToDelete, note);
            }

            return Ok(id);
        }
    }
}
