using IncidentRegistrationService.Models;

namespace IncidentRegistrationService.Services
{
    public interface IIncidentCorrelationService
    {
        Task<bool> CheckIncidentDuplicate(Incident incident);
    }
}
