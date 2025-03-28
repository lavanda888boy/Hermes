using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;
using IncidentRegistrationService.Responses;
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

        public AdminIncidentRegistrationController(IRepository<Incident> incidentRepository) 
        { 
            _incidentRepository = incidentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncidentsForValidation()
        {
            var incidents = await _incidentRepository.GetFilteredAsync();

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
            throw new NotImplementedException();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateOrValidateIncidentDetails([FromBody] Incident incident)
        {
            throw new NotImplementedException();
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
