using IncidentRegistrationService.Models;

namespace IncidentRegistrationService.Services
{
    public interface INotificationTransmissionService
    {
        Task SendIncidentNotification(Incident incident, string note = "");
    }
}
