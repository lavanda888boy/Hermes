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
        private readonly INotificationTransmissionService _notificationTransmissionService;

        public UserIncidentRegistrationController(IRepository<Incident> incidentRepository,
            INotificationTransmissionService notificationTransmissionService,
            IIncidentCorrelationService incidentCorrelationService)
        {
            _incidentRepository = incidentRepository;
            _notificationTransmissionService = notificationTransmissionService;
            _incidentCorrelationService = incidentCorrelationService;
        }

        [HttpGet("{userToken}")]
        public async Task<IActionResult> GetAllIncidentsHavingAffectedTheUser(string userToken, [FromQuery] int timestampDays)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewIncident([FromBody] Incident incident)
        {
            throw new NotImplementedException();
        }
    }
}
