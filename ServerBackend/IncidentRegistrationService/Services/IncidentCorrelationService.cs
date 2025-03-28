using IncidentRegistrationService.Models;
using IncidentRegistrationService.Repository;

namespace IncidentRegistrationService.Services
{
    public class IncidentCorrelationService : IIncidentCorrelationService
    {
        private readonly IRepository<Incident> _incidentRepository;

        public IncidentCorrelationService(IRepository<Incident> incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        public async Task<bool> CheckIncidentDuplicate(Incident incident)
        {
            throw new NotImplementedException();
        }
    }
}
