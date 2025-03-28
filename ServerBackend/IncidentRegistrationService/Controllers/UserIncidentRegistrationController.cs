using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace IncidentRegistrationService.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserIncidentRegistrationController : ControllerBase
    {
        private readonly IRepository<Incident> _incidentRepository;

        public UserIncidentRegistrationController(IRepository<Incident> incidentRepository)
        {
            _incidentRepository = incidentRepository;
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
